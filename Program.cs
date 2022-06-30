using Yandex;


Console.WriteLine("Hello, World!");
string login = "koroteeww@yandex.ru";
string pass = "3E8cUZaJnRsD3B94kuuM";
string oauthToken = "AQAAAAAFqAjTAAG8XotGIt5YD09quyr6xCmTbnE";

string funcurl = "https://functions.yandexcloud.net/d4entjkagd3fqn0hum1d";

YandexMusicClient client = new YandexMusicClient();
var res = client.Authorize(login, pass);
var isAuth = client.IsAuthorized;
var token = client.Token;
//var plPeronal = client.GetPersonalPlaylists();
var plFavs = client.GetFavorites();

string names = "";
foreach (var playlist in plFavs)
{
    names = names + "<"+playlist.Title + ">" + Environment.NewLine;
}
Console.WriteLine(names);

Console.ReadLine();