# Our.Umbraco.DataTypeConverter

> This code is provided as is, please review before using and make sure to back up your data. It managed to help us out on a project and perhaps will help you.

Adds a dashboard to allow you to overwrite the stored values within the database for property data (cmsPropertyData).

This is particularly useful when you have old data that may not be compatible with the latest version of Umbraco, or you want to switch to a different format.

Do not be alarmed if it seems to take ages as there may be a lot of data, however do **use with caution!**.

## Included converters

- MNTP Converter (XML to CSV)
- URL Picker Converter (XML to JSON)

## How to use

Here's what to do if you want to convert one of your data types to another without loss of data.

1. Find the data type that you want to convert.
2. Change that to use the data type that you want. (at this stage, the back office will stop rendering content as the existing data is incompatible with the new format)
3. Load up the dashboard for this package.
4. Select a data type to convert. (this is the one you just changed)
5. That will show a list of doc types where it is used, select doc type.
6. The conversion will begin and report back.
7. Repeat for other doc types. (yeah, there should be a select all option)

## Adding your own

Below is an example of how you would create your own converter.

```
public class MyConverter : IDataTypeConverter
{
    public string Name => "My Converter Name";

    public string PropertyEditorAlias => "AliasToChangeTo";

    public string Convert(string input)
    {
        // implement conversion code
    }
}
```

## Contributions

It would be great if this could be updated to become a proper tool for converting data types. As it stands it's a bit rough and ready. It's using a user control, which isn't exactly the best way of doing things in Umbraco these days. It could do with some other converters. It might be doing things in a messed up way and it needs a complete rework. 

Any support and suggestions welcome.