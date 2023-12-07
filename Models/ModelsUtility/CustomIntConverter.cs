using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.TypeConversion;

public class CustomIntConverter : CsvHelper.TypeConversion.Int32Converter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (int.TryParse(text, out var result))
            return result;

        // just cast decimal as int to remove trailing values
        if (decimal.TryParse(text, out var decimalResult))
            return (int)decimalResult;
        return null;
        
    }
}