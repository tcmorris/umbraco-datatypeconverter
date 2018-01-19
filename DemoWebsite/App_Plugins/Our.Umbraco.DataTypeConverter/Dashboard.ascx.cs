using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using log4net;
using Umbraco.Core.Models;
using Umbraco.Web.UI.Controls;
using Our.Umbraco.DataTypeConverter.Config;
using Our.Umbraco.DataTypeConverter.Converters;
using Our.Umbraco.DataTypeConverter.Services;

namespace Our.Umbraco.DataTypeConverter
{
    public class AffectedContent
    {
        public IContent Content { get; set; }
        public PropertyType PropertyType { get; set; }
    }

    public partial class Dashboard : UmbracoUserControl
    {
        private DataTypeConverterConfig _importConfig;
        private IImportService _importService;      
        private ILog _logger;
        private int _count;

        private List<IContentType> _affectedDocTypes;
        private List<AffectedContent> _affectedContent;
        private IDataTypeConverter _converter;

        public int DataTypeId => int.Parse(ddlDataTypes.SelectedValue);

        public int DocTypeId => int.Parse(ddlDocTypes.SelectedValue);

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // logout the user if invalid
            if (UmbracoContext.Security.ValidateCurrentUser() == false)
            {
                //ensure the person is definitely logged out
                UmbracoContext.Security.ClearCurrentLogin();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // set up import service
            _importConfig = new DataTypeConverterConfig();
            _importService = new ImportService();         
            _converter = FindValidConverter(DataTypeId);
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            pnlAlert.Visible = false;

            if (!Page.IsPostBack)
            {
                lblConverters.Text = string.Join(", ", _importService.GetAllConverters().Select(x => x.Name).ToList());
                GetDataTypes();
            }
        }

        public void DisplayAlert(string message, string cssClass)
        {
            pnlAlert.Visible = true;
            pnlAlert.CssClass = cssClass;
            lblMessage.Text = message;
        }

        public void GetDataTypes()
        {
            var dataTypes = Services.DataTypeService.GetAllDataTypeDefinitions();

            // populate dropdown list
            AddSelectItem(ddlDataTypes, "-- Select --");
            foreach (var dtd in dataTypes.OrderBy(x => x.Name))
            {
                if (FindValidConverter(dtd.Id) != null)
                {
                    var dataType = new ListItem
                    {
                        Value = dtd.Id.ToString(),
                        Text = dtd.Name
                    };

                    ddlDataTypes.Items.Add(dataType);
                }
            }
        }

        public void GetDocTypes(int dataTypeId)
        {
            // get doc types using data type
            _affectedDocTypes = new List<IContentType>();
            var parentDocTypes = new List<IContentType>();
            parentDocTypes.AddRange(Services.ContentTypeService.GetAllContentTypes().Where(x => x.PropertyTypes.Any(y => y.DataTypeDefinitionId == dataTypeId)));

            // add their children
            foreach (var parentDocType in parentDocTypes)
            {
                _affectedDocTypes.AddRange(Services.ContentTypeService.GetContentTypeChildren(parentDocType.Id));
            }

            // add in parents
            _affectedDocTypes.AddRange(parentDocTypes);

            // populate dropdown list
            AddSelectItem(ddlDocTypes, "-- All --");
            foreach (var dt in _affectedDocTypes.OrderBy(x => x.Name))
            {
                var docType = new ListItem
                {
                    Value = dt.Id.ToString(),
                    Text = dt.Name
                };
                ddlDocTypes.Items.Add(docType);
            }
        }

        private void AddSelectItem(DropDownList ddl, string optionText)
        {
            ddl.Items.Clear();
            var selectItem = new ListItem
            {
                Value = "0",
                Text = optionText
            };
            ddl.Items.Add(selectItem);
        }

        public IDataTypeConverter FindValidConverter(int dataTypeId)
        {
            // find the data type and it's property editor alias
            var dataType = Services.DataTypeService.GetDataTypeDefinitionById(dataTypeId);
            if (dataType == null)
                return null;

            // find a converter that supports that alias
            var converter = _importService.GetConverter(dataType.PropertyEditorAlias);
            return converter;
        }

        /// <summary>
        /// Estimate number of entries to update
        /// </summary>
        public void GetAffectedContent()
        {
            _affectedContent = new List<AffectedContent>();

            if (DocTypeId == 0)
            {
                // check all doc types
                GetDocTypes(DataTypeId);

                foreach (var docType in _affectedDocTypes)
                {
                    foreach (var content in Services.ContentService.GetContentOfContentType(docType.Id).Where(x => !x.Trashed))
                    {
                        foreach (var propertyType in content.PropertyTypes.Where(x => x.DataTypeDefinitionId == DataTypeId))
                        {
                            _affectedContent.Add(new AffectedContent()
                            {
                                Content = content,
                                PropertyType = propertyType
                            });
                        }
                    }
                }
            }
            else
            {
                // check selected doc type
                foreach (var content in Services.ContentService.GetContentOfContentType(DocTypeId).Where(x => !x.Trashed))
                {
                    foreach (var propertyType in content.PropertyTypes.Where(x => x.DataTypeDefinitionId == DataTypeId))
                    {
                        _affectedContent.Add(new AffectedContent()
                        {
                            Content = content,
                            PropertyType = propertyType
                        });
                    }
                }
            }

            // crude way of storing this data
            HttpContext.Current.Session["affectedContent"] = _affectedContent;

            DisplayAlert($"Estimated {_affectedContent.Count} properties to update. Will use {_converter.Name}", "alert alert-info");
            btnConvert.Enabled = true;
        }

        protected void ddlDataTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDocTypes(DataTypeId);
        }

        protected void ddlDocTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetAffectedContent();
        }

        protected void btnConvert_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["affectedContent"] == null)
            {
                DisplayAlert("No valid data for the session.", "alert alert-warning");
            }

            _logger.Info($"Begin conversion of {ddlDataTypes.SelectedItem.Text}");
            _count = 0;

            try
            {
                _affectedContent = HttpContext.Current.Session["affectedContent"] as List<AffectedContent>;

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                // convert the content
                foreach (var data in _affectedContent)
                {
                    // run conversion
                    var oldValue = data.Content.GetValue<string>(data.PropertyType.Alias);
                    var result = _importService.ConvertContent(_converter, oldValue);
                    if (result != null && result.IsSuccess)
                    {
                        // update content with the new value
                        data.Content.SetValue(data.PropertyType.Alias, result.NewValue);
                        Services.ContentService.Save(data.Content);

                        _logger.Debug(
                            $"Converted {data.PropertyType.Alias} from {result.OldValue} to {result.NewValue} on page {data.Content.Id}");
                        _count++;
                    }
                }

                // add some logging
                var docTypesUsed = string.Join(", ", _affectedContent.Select(x => x.Content.ContentType.Name).Distinct().ToList());
                _logger.Info(
                    $"Finished {_count} conversion(s) out of possible {_affectedContent.Count} values. Doc types used: {docTypesUsed}");

                // republish
                if (_importConfig.ShouldRepublish)
                {
                    Services.ContentService.RePublishAll();
                }

                // hoorah!
                stopWatch.Stop();
                DisplayAlert(
                    $"That seemed to work! Converted {_count} out of possible {_affectedContent.Count} values in {stopWatch.Elapsed}. Please check your content to confirm.", 
                    "alert alert-success");
            }
            catch (Exception ex)
            {
                // log error
                _logger.Error($"Exception: {ex.Message}", ex);
                DisplayAlert("Hmm, that didn't seem to work. Please try again.", "alert alert-error");
            }
        }
    }
}