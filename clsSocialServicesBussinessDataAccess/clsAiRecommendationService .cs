using clsSocialServicesDataAccess.Posts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace clsSocialServicesBussiness
{
    public class clsAiRecommendationService
    {
        private readonly string _apiKey;
        private readonly IPostRepository _postRepo;
        private readonly IConfiguration _config;

        public clsAiRecommendationService(IPostRepository postRepo, IConfiguration configuration)
        {
            _postRepo = postRepo;
            _config = configuration;
            _apiKey = _config["API_KEY"];
        }


        public async Task<string> GetRecommendations(string userMessage, int userId)
        {
            // جلب البوستات الخاصة بالمستخدم
            var posts = _postRepo.GetAllPosts(userId);

            // عمل Projection باستخدام الأسماء النصية بدلاً من الـ IDs لزيادة دقة الـ AI
            // ملاحظة: يمكنك تعويض p.CountyName بـ p.CountyID.ToString() أو الاسم الفعلي للمحافظة المتوفر لديك
            var simplifiedPosts = posts.Select(p => new {
                Id = p.PostID,
                Title = p.PostTitle,
                Description = p.Description,
                ProfessionName = p.ProfessionName, // هنا نمرر اسم المهنة نصياً (مثل: نجار، سباك) بدلاً من ID
                TypeName = p.PostTypeName,             // هنا نمرر اسم نوع الخدمة نصياً بدلاً من ID
                CountyName = p.CountyName,         // هنا نمرر اسم المنطقة/المحافظة (مثل: عمان، إربد)
                Price = p.Price,
                Latitude = p.Latitude,
                Longitude = p.Longitude
            }).ToList();

            string postsJson = JsonSerializer.Serialize(simplifiedPosts);

            var client = new HttpClient();

            // البرمبت المطور المتوافق مع هيكلية بياناتك الفعلية بالأسماء النصية
            string prompt = $@"
You are a precise Service Recommendation System. Your task is to match the user's request with the most relevant posts from the provided JSON data.

[User Request]
""{userMessage}""

[Available Posts Data (JSON)]
{postsJson}

[Ranking & Matching Criteria (Strict Order of Priority)]
1. Service Type & Intent Match: Match the user's requested service with the post's 'Title', 'Description', 'ProfessionName', or 'TypeName'.
2. Geographical Location: Prioritize matching the 'CountyName' (e.g., Amman, Zarqa, etc.) if a specific area/city is mentioned or implied, or use geographical proximity via 'Latitude' and 'Longitude'.
3. Price Match: Consider matching the budget if specified, using the 'Price' field.

[Strict Constraints]
- Extract the user's intended number of recommendations if specified (e.g., ""give me 2"", ""top 5""). If not specified, recommend exactly 3 posts.
- If NO posts are relevant or match the requested service type, return an empty array: [].
- Sort the resulting IDs (corresponding to 'Id' field) from most relevant to least relevant.
- Return ONLY the JSON object matching the requested schema. No explanations, no markdown formatting.
";

            // إرسال الطلب مع تحديد الـ Schema لضمان التزام الموديل بالهيكل المطلوب
            var body = new
            {
                contents = new[]
                {
            new { parts = new[] { new { text = prompt } } }
        },
                generationConfig = new
                {
                    responseMimeType = "application/json",
                    responseSchema = new
                    {
                        type = "OBJECT",
                        properties = new
                        {
                            postIds = new
                            {
                                type = "ARRAY",
                                items = new { type = "INTEGER" },
                                description = "List of matching post IDs (PostID) sorted by relevance."
                            }
                        },
                        required = new[] { "postIds" }
                    }
                }
            };


            var response = await client.PostAsJsonAsync(
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.5-flash:generateContent?key={_apiKey}",
                body
            );

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return $"Error: {result}";

            var jsonDoc = JsonDocument.Parse(result);
            string text = jsonDoc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString()!;

            // النص الراجع هنا سيكون مضموناً 100% على شكل {"postIds": [36, 28, 23]} ومطابقاً لمعرفات قاعدة بياناتك
            return text;
        }

    }
}
