using Azure;
using Azure.AI.OpenAI;
using System.Diagnostics;
using static System.Environment;

namespace DALLE
{
    public class Program
    {
        // add an async Main method:
        public static async Task Main(string[] args)
        {

            string endpoint = "";
            string key = "";

            OpenAIClient client = new(new Uri(endpoint), new AzureKeyCredential(key));


            Response<ImageGenerations> imageGenerations = await client.GetImageGenerationsAsync(
                new ImageGenerationOptions()
                {
                    DeploymentName = "Dalle3",
                    Prompt = "Em uma praia cristalina brasileira, esta um casal o homem tem cabelo loiro e pele clara ela de cabelos castanhos e pele bronzeada, abraçados, eles estao contemplando o por do sol enquanto apreciam as ondas do mar, ao redor do mar a natureza tropical é evidente, a horientacao da imagem mostra o casal de costas com o mar ao fundo",
                    Size = ImageSize.Size1024x1024,
                });

            // Image Generations responses provide URLs you can use to retrieve requested images
            Uri imageUri = imageGenerations.Value.Data[0].Url;

            // Print the image URI to console:
            Console.WriteLine(imageUri);

            // Open the image URL in the default web browser
            Process.Start(new ProcessStartInfo
            {
                FileName = imageUri.AbsoluteUri,
                UseShellExecute = true
            });

            Console.ReadKey();

        }
    }
}
