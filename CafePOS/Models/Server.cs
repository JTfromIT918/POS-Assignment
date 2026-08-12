namespace CafePOS.Models;

public class Server
{
    public int ServerID { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime HireDate { get; set; } 

    public DateTime? TermDate { get; set; }
}