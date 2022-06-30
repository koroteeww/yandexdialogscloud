using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Function
{
    public class AliceRequestBase
    {


        [JsonPropertyName("meta")]
        public AliceMetaModel Meta { get; set; }

        [JsonPropertyName("session")]
        public AliceSessionModel Session { get; set; }

        [JsonPropertyName("request")]
        public AliceRequestModel Request { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
    public class AliceMetaModel
    {
        [JsonPropertyName("locale")]
        public string Locale { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("interfaces")]
        public AliceInterfacesModel Interfaces { get; set; }
    }

    public class AliceInterfacesModel
    {
        [JsonPropertyName("screen")]
        public object Screen { get; set; }

        [JsonPropertyName("payments")]
        public object Payments { get; set; }

        [JsonPropertyName("account_linking")]
        public object AccountLinking { get; set; }


    }

    public class AliceSessionModel
    {
        [JsonPropertyName("new")]
        public bool New { get; set; }

        [JsonPropertyName("session_id")]
        public string SessionId { get; set; }

        [JsonPropertyName("message_id")]
        public int MessageId { get; set; }

        [JsonPropertyName("skill_id")]
        public string SkillId { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("user")]
        public AliceSessionUserModel User { get; set; }

        [JsonPropertyName("application")]
        public AliceSessionApplicationModel Application { get; set; }


    }
    public class AliceSessionUserModel
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
    public class AliceSessionApplicationModel
    {
        [JsonPropertyName("application_id")]
        public string ApplicationId { get; set; }
    }

    public class AliceRequestModel
    {
        [JsonPropertyName("command")]
        public string Command { get; set; }

        [JsonPropertyName("original_utterance")]
        public string OriginalUtterance { get; set; }

        [JsonPropertyName("payload")]
        public object Payload { get; set; }

        [JsonPropertyName("markup")]
        public AliceMarkupModel Markup { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        //nlu
        [JsonPropertyName("nlu")]
        public AliceNluModel Nlu { get; set; }
    }
    #region NLU
    public class AliceMarkupModel
    {
        [JsonPropertyName("dangerous_context")]
        public bool DangerousContext { get; set; }
    }
    //nlu
    public class AliceNluModel
    {
        [JsonPropertyName("tokens")]
        public IEnumerable<string> Tokens { get; set; }

        [JsonPropertyName("entities")]
        [JsonConverter(typeof(AliceEntityModelEnumerableConverter))]
        public IEnumerable<AliceEntityModel> Entities { get; set; }



        [JsonPropertyName("intents")]
        public object Intents { get; set; }
    }
    #region nluModels
    public class AliceEntityNumberModel : AliceEntityModel
    {
        [JsonPropertyName("value")]
        public double Value { get; set; }
    }
    public class AliceEntityGeoModel : AliceEntityModel
    {
        [JsonPropertyName("value")]
        public AliceEntityGeoValueModel Value { get; set; }
    }
    public class AliceEntityGeoValueModel
    {
        [JsonPropertyName("house_number")]
        public string HouseNumber { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("airport ")]
        public string Airport { get; set; }
    }
    public class AliceEntityFioModel : AliceEntityModel
    {
        [JsonPropertyName("value")]
        public AliceEntityFioValueModel Value { get; set; }
    }
    public class AliceEntityFioValueModel
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
        [JsonPropertyName("patronymic_name")]
        public string PatronymicName { get; set; }
    }
    public class AliceEntityStringModel : AliceEntityModel
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
    public class AliceEntityDateTimeModel : AliceEntityModel
    {
        [JsonPropertyName("value")]
        public AliceEntityDateTimeValueModel Value { get; set; }
    }
    public class AliceEntityDateTimeValueModel
    {
        [JsonPropertyName("day")]
        public double Day { get; set; }

        [JsonPropertyName("day_is_relative")]
        public bool DayIsRelative { get; set; }

        [JsonPropertyName("hour")]
        public double Hour { get; set; }

        [JsonPropertyName("hour_is_relative")]
        public bool HourIsRelative { get; set; }

        [JsonPropertyName("minute")]
        public double Minute { get; set; }

        [JsonPropertyName("minute_is_relative")]
        public bool MinuteIsRelative { get; set; }

        [JsonPropertyName("month")]
        public double Month { get; set; }

        [JsonPropertyName("month_is_relative")]
        public bool MonthIsRelative { get; set; }

        [JsonPropertyName("year")]
        public double Year { get; set; }

        [JsonPropertyName("year_is_relative")]
        public bool YearIsRelative { get; set; }
    }
    #endregion
    public class AliceEntityModelEnumerableConverter : EnumerableConverter<AliceEntityModel>
    {
        private static readonly Dictionary<string, Type> _typeMap = new Dictionary<string, Type>
        {
            { "YANDEX.GEO", typeof(AliceEntityGeoModel) },
            { "YANDEX.FIO", typeof(AliceEntityFioModel) },
            { "YANDEX.NUMBER", typeof(AliceEntityNumberModel) },
            { "YANDEX.DATETIME", typeof(AliceEntityDateTimeModel) },
            { "YANDEX.STRING", typeof(AliceEntityStringModel) },
        };
        protected override AliceEntityModel ToItem(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            var readerAtStart = reader;

            string type = null;
            using (var jsonDocument = JsonDocument.ParseValue(ref reader))
            {
                var jsonObject = jsonDocument.RootElement;

                type = jsonObject
                    .EnumerateObject()
                    .FirstOrDefault(x => x.Name == "type")
                    .Value.GetString();
            }
            if (!string.IsNullOrEmpty(type) && _typeMap.TryGetValue(type, out var targetType))
            {
                return JsonSerializer.Deserialize(ref readerAtStart, targetType, options) as AliceEntityModel;
            }
            return null;
        }

        protected override void WriteItem(Utf8JsonWriter writer, AliceEntityModel item, JsonSerializerOptions options)
        {
            object newValue = null;
            if (item != null)
            {
                newValue = Convert.ChangeType(item, item.GetType(), CultureInfo.InvariantCulture);
            }

            JsonSerializer.Serialize(writer, newValue, options);
        }
    }
    public abstract class EnumerableConverter<TItem> : JsonConverter<IEnumerable<TItem>>
    {
        public override IEnumerable<TItem> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.StartArray:
                    var list = new List<TItem>();
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray)
                        {
                            break;
                        }

                        list.Add(ToItem(ref reader, options));
                    }

                    return list.ToArray();
                case JsonTokenType.None:
                case JsonTokenType.StartObject:
                case JsonTokenType.EndObject:
                case JsonTokenType.EndArray:
                case JsonTokenType.PropertyName:
                case JsonTokenType.Comment:
                case JsonTokenType.String:
                case JsonTokenType.Number:
                case JsonTokenType.True:
                case JsonTokenType.False:
                case JsonTokenType.Null:
                default:
                    return Array.Empty<TItem>();
            }
        }

        public override void Write(Utf8JsonWriter writer, IEnumerable<TItem> value, JsonSerializerOptions options)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (value == null)
            {
                return;
            }

            writer.WriteStartArray();
            foreach (var item in value)
            {
                WriteItem(writer, item, options);
            }

            writer.WriteEndArray();
        }

        protected abstract TItem ToItem(ref Utf8JsonReader reader, JsonSerializerOptions options);

        protected virtual void WriteItem(Utf8JsonWriter writer, TItem item, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, item, options);
        }
    }
    public class AliceEntityModel
    {
        [JsonPropertyName("tokens")]
        public NluTokens Tokens { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }



    public class NluTokens
    {
        [JsonPropertyName("start")]
        public int start { get; set; }
        [JsonPropertyName("end")]
        public int end { get; set; }

    }



    #endregion



    public class AliceResponse
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("response")]
        public AliceResponseModel Response { get; set; }
        public AliceResponse()
        {
            Version = "1.0";
            Response = new AliceResponseModel();
        }
        public AliceResponse(AliceRequestBase request)
        {
            string funcurl = "https://functions.yandexcloud.net/d4entjkagd3fqn0hum1d";
            Version = request.Version;
            Response = new AliceResponseModel();
            Response.EndSession = false;
            var sh = new ShowMeta();
            //some random guid
            sh.content_id = "88f83f54-1135-4238-85c4-5e45959a64d0";
            sh.id = "88f83f54-1135-4238-85c4-5e45959a64d0";
            sh.publication_date = DateTime.UtcNow.ToString("o");
            string reqC = request.Request.Command;
            if (reqC == "привет")
            {
                reqC = reqC + " медвед ";
            }

            string help = " вот что я умею: скажи чегототам и я сделаю что-нибудь.";
            Response.ShowItemMeta = sh;
            if (reqC == string.Empty ||
                reqC.ToLower() == "помощь" ||
                reqC.ToLower().Contains("что ты умеешь"))
            {

                Response.Text = "Привет! " + help;
            }
            else if (reqC.ToLower().Contains("авторизация"))
            {
                string auth = "https://oauth.yandex.ru/authorize?response_type=token&client_id=e738f647b8b145948b7a5a622a59819d&redirect_uri="
                    + funcurl;

                Response.Text = "попробуй эту ссылку " + auth;

            }
            else
            {
                //извлечение чисел из входящего запроса
                var ints = request.Request.Nlu.Entities.Where(t => t.Type == "YANDEX.NUMBER").ToList();
                string sints = "";
                if (ints != null && ints.Count > 0)
                {
                    sints = ". А еще вот числа из запроса: ";
                    foreach (var item in ints)
                    {
                        if (item != null)

                            sints = sints + " " + (item as AliceEntityNumberModel).Value;


                    }

                }
                //попугай

                Response.Text = "ПРИВЕТ ПОВЕЛИТЕЛЬ! вот что прислали: " + reqC + sints;
                Response.Tts = Response.Text;
            }

        }
    }
    public class AliceResponseModel
    {
        private string _tts;
        private const int _textMaxLength = 1024;
        private const int _ttsMaxLength = 1024;

        private string _text;

        [JsonPropertyName("text")]
        public string Text
        {
            get => _text;

            set
            {
                _text = value;
            }
        }
        [JsonPropertyName("tts")]
        public string Tts
        {
            get => _tts;

            set
            {
                _tts = value;
            }
        }

        [JsonPropertyName("end_session")]
        public bool EndSession { get; set; }

        [JsonPropertyName("show_item_meta")]
        public ShowMeta ShowItemMeta { get; set; }

    }
    public class ShowMeta
    {
        [JsonPropertyName("publication_date")]
        public string publication_date { get; set; }
        [JsonPropertyName("id")]
        public string id { get; set; }
        [JsonPropertyName("content_id")]
        public string content_id { get; set; }
    }

    public class Handler
    {
        public AliceResponse FunctionHandler(string requestJ)
        {

            if (requestJ.Contains("version") == false)
            {
                if (requestJ.Contains("queryStringParameters"))
                {
                    int startQ = requestJ.IndexOf("queryStringParameters");
                    int start2 = requestJ.IndexOf("code", startQ);
                    int startCode = requestJ.IndexOf(":", start2);
                    int endCode = requestJ.IndexOf("}", start2);
                    string code = requestJ.Substring(startCode, endCode - startCode);
                    code = code.Replace("'", "");
                    code = code.Replace(":", "");
                    code = code.Replace("}", "");
                    code = code.Replace("u0022", "");
                    code = code.Replace("\\", "");
                    code = code.Replace("\'", "");
                    code = code.Replace("\"", "");
                    //int icode = Convert.ToInt32(code);
                    throw new Exception("code=" + code);
                }
                else
                    //hmmm
                    throw new Exception("req=" + requestJ);

                var response = new AliceResponse();
                //response.Version = "1.0";
                //response.Response = new AliceResponseModel();
                response.Response.EndSession = false;
                var sh = new ShowMeta();
                //some random guid
                sh.content_id = "88f83f54-1135-4238-85c4-5e45959a64d0";
                sh.id = "88f83f54-1135-4238-85c4-5e45959a64d0";
                sh.publication_date = DateTime.UtcNow.ToString("o");
                response.Response.ShowItemMeta = sh;
                response.Response.Text = "Привет!" + requestJ;
                return response;
            }
            else
            {
                AliceRequestBase request = JsonSerializer.Deserialize<AliceRequestBase>(requestJ);
                var response = new AliceResponse(request);
                return response;
            }
        }
        public string token(string access_token, string expires_in = "", string token_type = "")
        {
            string res = access_token;
            if (res.Contains("#"))
                res = res.Substring(res.IndexOf("#"));
            //hmmm
            throw new Exception(res);

            return res;
        }
    }
}