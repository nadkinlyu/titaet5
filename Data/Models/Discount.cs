namespace Data.Models;

public class Discount
{
    public long Id { get; set; }
    public string name { get; set; } = string.Empty;
    public double Value { get; set; } = 1;
    public List<Card> Cards { get; set; } 
}