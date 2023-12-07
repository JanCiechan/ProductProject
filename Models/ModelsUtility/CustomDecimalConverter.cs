using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.TypeConversion;

public class CustomDecimalConverter : CsvHelper.TypeConversion.DecimalConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        
        if (decimal.TryParse(text.Replace(',', '.'), out var result))
            return result;
        else
        {
            return null;
        }

    }
}