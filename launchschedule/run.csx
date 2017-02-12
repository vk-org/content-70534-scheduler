#r "Newtonsoft.Json"

using System.Net;
using Newtonsoft.Json;

public static HttpResponseMessage Run(HttpRequestMessage req, ICollector<Launch> outputTable, TraceWriter log)
{
    log.Info("Launch list update request received.");

    var targetDate = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd");

    var blastoffs = GetLaunchesForDate(targetDate);

    if(blastoffs.total > 0) {
        foreach(var launch in blastoffs.launches) {
            launch.PartitionKey = targetDate;
            launch.RowKey = launch.id.ToString();
            outputTable.Add(launch);
            log.Info($"Added launch row {launch.RowKey} to partition {launch.PartitionKey}");
        }
    }

    var response = req.CreateResponse(HttpStatusCode.OK);
    response.Content = new StringContent(JsonConvert.SerializeObject(blastoffs, Formatting.Indented), System.Text.Encoding.UTF8, "application/json");
   
    return response;
}

private static LaunchList GetLaunchesForDate(string targetDate) {
    var output = new LaunchList();

    try
    {
        var request = (HttpWebRequest)WebRequest.Create($"https://launchlibrary.net/1.2/launch/{targetDate}/{targetDate}");
        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.10136";
        using (var response = (HttpWebResponse)request.GetResponse())
        {
            using (var responseStream = response.GetResponseStream())
            {
                using(var reader = new StreamReader(responseStream)) 
                {
                    var data = reader.ReadToEnd();
                    output = JsonConvert.DeserializeObject<LaunchList>(data);
                }
            }
        }
    }
    catch (Exception) {}

    return output;
}

public class Pad
{
    public int id { get; set; }
    public string name { get; set; }
    public string infoURL { get; set; }
    public string wikiURL { get; set; }
    public string mapURL { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }
    public List<object> agencies { get; set; }
}

public class Location
{
    public List<Pad> pads { get; set; }
    public int id { get; set; }
    public string name { get; set; }
    public string infoURL { get; set; }
    public string wikiURL { get; set; }
    public string countryCode { get; set; }
}

public class Rocket
{
    public int id { get; set; }
    public string name { get; set; }
    public string configuration { get; set; }
    public string familyname { get; set; }
    public List<object> agencies { get; set; }
    public string wikiURL { get; set; }
    public List<object> infoURLs { get; set; }
    public List<int> imageSizes { get; set; }
    public string imageURL { get; set; }
}

public class Launch
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public int id { get; set; }
    public string name { get; set; }
    public string windowstart { get; set; }
    public string windowend { get; set; }
    public string net { get; set; }
    public int wsstamp { get; set; }
    public int westamp { get; set; }
    public int netstamp { get; set; }
    public string isostart { get; set; }
    public string isoend { get; set; }
    public string isonet { get; set; }
    public int status { get; set; }
    public int inhold { get; set; }
    public int tbdtime { get; set; }
    public List<string> vidURLs { get; set; }
    public string vidURL { get; set; }
    public List<object> infoURLs { get; set; }
    public object infoURL { get; set; }
    public object holdreason { get; set; }
    public object failreason { get; set; }
    public int tbddate { get; set; }
    public int probability { get; set; }
    public object hashtag { get; set; }
    public Location location { get; set; }
    public Rocket rocket { get; set; }
    public List<object> missions { get; set; }

    public Launch() 
    {
        PartitionKey = string.Empty;
        RowKey = "0";
    }
}

public class LaunchList
{
    public int total { get; set; }
    public List<Launch> launches { get; set; }
    public int offset { get; set; }
    public int count { get; set; }
}