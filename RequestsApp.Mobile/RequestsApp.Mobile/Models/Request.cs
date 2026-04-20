using System;
using System.Text.Json.Serialization;

namespace RequestsApp.Mobile.Models
{
    public class Request
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("request_number")]
        public string RequestNumber { get; set; } = string.Empty;

        [JsonPropertyName("citizen_name")]
        public string CitizenName { get; set; } = string.Empty;

        [JsonPropertyName("citizen_phone")]
        public string CitizenPhone { get; set; } = string.Empty;

        [JsonPropertyName("category_name")]
        public string CategoryName { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("status_name")]
        public string StatusName { get; set; } = string.Empty;

        private DateTime _createdDate = DateTime.Now;

        [JsonPropertyName("created_date")]
        public DateTime CreatedDate
        {
            get => _createdDate;
            set
            {
                // Если дата минимальная или null, ставим текущую
                if (value == default || value.Year < 2000)
                    _createdDate = DateTime.Now;
                else
                    _createdDate = value;
            }
        }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; } = string.Empty;
    }
}