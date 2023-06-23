using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Translation;

public class Program
{
    static readonly string SubscriptionKey = ""; // Your subscription key
    static readonly string Region = ""; // Your subscription service region
    private const string FromLanguage = "es-MX";
    private static readonly List<string> ToLanguages = new List<string> { "en-US", "fr", "de" };

    public static async Task Main()
    {
        Console.WriteLine("Starting...");
        await TranslateSpeechToTextAsync();
    }

    private static async Task TranslateSpeechToTextAsync()
    {
        var translationConfig = SpeechTranslationConfig.FromSubscription(SubscriptionKey, Region);
        translationConfig.SpeechRecognitionLanguage = FromLanguage;
        ToLanguages.ForEach(translationConfig.AddTargetLanguage);

        using var recognizer = new TranslationRecognizer(translationConfig);

        Console.WriteLine("Say something in: " + FromLanguage);
        Console.WriteLine($"We'll translate into '{string.Join("', '", ToLanguages)}'.\n");

        try
        {
            var result = await recognizer.RecognizeOnceAsync();

            if (result.Reason == ResultReason.TranslatedSpeech)
            {
                Console.WriteLine($"Recognized: \"{result.Text}\":");
                foreach (var (language, translation) in result.Translations)
                {
                    Console.WriteLine($"Translated into '{language}': {translation}");
                }
            }
            else if (result.Reason == ResultReason.RecognizedSpeech)
            {
                Console.WriteLine($"Recognized: {result.Text}");
            }
            else if (result.Reason == ResultReason.NoMatch)
            {
                Console.WriteLine($"NOMATCH: Speech could not be recognized.");
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = CancellationDetails.FromResult(result);
                Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");
                if (cancellation.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                    Console.WriteLine($"CANCELED: Did you update the subscription info?");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}