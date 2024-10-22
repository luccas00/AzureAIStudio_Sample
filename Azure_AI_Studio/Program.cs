using Azure.AI.OpenAI;
using Azure;
using static System.Environment;
using Azure.AI.Language.Conversations;
using System.Net;

namespace ChatGPT
{
    public class Program
    {
        private static string endpoint = "";
        private static string key = "";

        //Altere a task para que seja uma conversa natural, perguntas e respostas ate o usuario decidir encerrar
        public static async Task Main(string[] args)
        {
            OpenAIClient client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));

            Console.WriteLine("Bem-vindo ao Chatbot! Digite 'Encerrar Chat' para sair.");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\nVocê: ");
                Console.ResetColor();

                string userInput = Console.ReadLine();

                if (userInput.Equals("Encerrar Chat", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Chat encerrado. Até mais!");
                    break;
                }

                var chatCompletionsOptions = new ChatCompletionsOptions()
                {
                    DeploymentName = "gpt-35-turbo-16k",
                    Messages =
                    {
                        new ChatRequestSystemMessage("Você é um tradutor de portugues para ingles, todo o texto que voce recebe voce traduz e retorna para o usuario, caso tenham erros de ortografia ou gramatica corrija. Voce não da detalhes, apenas traduz todo o texto que recebe"),
                        new ChatRequestUserMessage(userInput),
                    },
                    MaxTokens = 100
                };

                Console.WriteLine();

                await foreach (StreamingChatCompletionsUpdate chatUpdate in client.GetChatCompletionsStreaming(chatCompletionsOptions))
                {
                    if (chatUpdate.Role.HasValue)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"{chatUpdate.Role.Value.ToString().ToUpperInvariant()}: ");
                        Console.ResetColor();
                    }
                    if (!string.IsNullOrEmpty(chatUpdate.ContentUpdate))
                    {
                        Console.Write(chatUpdate.ContentUpdate);
                    }
                }
                Console.WriteLine();
            }
        }
    }
}