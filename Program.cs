using Yandex;
using Yandex.Music.Api.Models.Track;

Console.WriteLine("Hello, World!");
string login = "koroteeww@yandex.ru";
string pass = "3E8cUZaJnRsD3B94kuuM";
string oauthToken = "AQAAAAAFqAjTAAG8XotGIt5YD09quyr6xCmTbnE";
string token2 = "AQAAAAAFqAjTAAgEMV2drlk1Sk3vqKvAQslYlLk";

string funcurl = "https://functions.yandexcloud.net/d4entjkagd3fqn0hum1d";

YandexMusicClient client = new YandexMusicClient();
//var res = client.Authorize(login, pass);
var res = client.Authorize(oauthToken);
var isAuth = client.IsAuthorized;
var token = client.Token;
var plPeronal = client.GetPersonalPlaylists();
var somePersPlaylis = plPeronal.Where(pl => pl.Title.ToLower() == "плейлист дня").First();
var tracksC1 = somePersPlaylis.Tracks.Count;

var plFavs = client.GetFavorites();
//этот плейлист без треков!
var myPl = plFavs.Where(pp => pp.Title.ToLower() == "утро энергия").First();
//а вот так вот он уже с треками!!! ура!!!
var testIt = client.GetPlaylist(login, myPl.Kind);
var testC = testIt.Tracks.Count;
//download
var track3 = testIt.Tracks[3];
//var trackMore = client.GetTrack(track3.Track.Id);
//var ff = client.GetDownInfo(trackMore.Id);
//var fdown = ff[0];
//var downP = client.TrackDownload(fdown);
var urla = client.BuildUrl(track3.Track);

//names start
Console.WriteLine("---");
string names = "";
List<YTrackContainer> tracksALL = new List<YTrackContainer>();
foreach (var playlist in plFavs)
{
    var more = client.GetPlaylist(login, playlist.Kind);
    names = names + "<" + playlist.Title + "> " + more.Tracks.Count + Environment.NewLine;
}
Console.WriteLine(names);
names = "";
Console.WriteLine("---");
foreach (var playlist in plPeronal)
{
    tracksALL.AddRange(playlist.Tracks);
    names = names + "<" + playlist.Title + ">" + playlist.Tracks.Count + Environment.NewLine;
}
Console.WriteLine(names);
Console.WriteLine("---");
//names end
//tracks shuffle
//List<Yandex.Music.Api.Models.Track.YTrack> tracks1 = new List<Yandex.Music.Api.Models.Track.YTrack>();
//foreach (var track in testIt.Tracks)
//{
//    tracks1.Add(track.Track);
//}
//var rnd = new Random();
//var randomizedTracks = tracks1.OrderBy(item => rnd.Next()).ToList();
//change in pl
//client.ChangePlaylistTracks(myPl, tracks1.ToArray() , randomizedTracks.ToArray() );
string name = "random_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
//var res3 = client.CreateWithTracks(name, randomizedTracks.ToArray());
//Random rng = new Random();
//int n = tracksALL.Count;
//while (n > 1)
//{
//    n--;
//    int k = rng.Next(n + 1);
//    var value = tracksALL[k];
//    tracksALL[k] = tracksALL[n];
//    tracksALL[n] = value;
//}
////
//List<YTrack> susus = new List<YTrack>();
//foreach (var item in tracksALL)
//{
//    susus.Add(item.Track);
//}
//var res3 = client.CreateWithTracks(name, susus.ToArray());
//Console.WriteLine("SHUFFLED "+res3.Tracks.Count);

Console.ReadLine();
