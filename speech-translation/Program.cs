using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;

string SPEECH__SUBSCRIPTION_KEY = ""; // Your subscription key
string SPEECH__REGION = ""; // Your subscription service region

var translationConfig = SpeechTranslationConfig.FromSubscription(SPEECH__SUBSCRIPTION_KEY, SPEECH__REGION);
var fromLanguage = "es-MX";
var toLanguages = new List<string> { "en-US", "fr", "de" };
translationConfig.SpeechRecognitionLanguage = fromLanguage;
toLanguages.ForEach(translationConfig.AddTargetLanguage);

var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
var speechRecognizer = new SpeechRecognizer(translationConfig, audioConfig);

speechRecognizer.Recognizing += (s, e) =>
{
    Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");
};
speechRecognizer.Recognized += (s, e) =>
{
    var result = e.Result;
    Console.WriteLine($"RECOGNIZED: Text={result.Text}");
};
speechRecognizer.Canceled += (s, e) =>
{
    Console.WriteLine($"CANCELED: Reason={e.Reason}");
    if (e.Reason == CancellationReason.Error)
    {
        Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
        Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
        Console.WriteLine($"CANCELED: Did you update the subscription info?");
    }
};
speechRecognizer.SessionStarted += (s, e) =>
{
    Console.WriteLine("\n    Session started event.");
};
speechRecognizer.SessionStopped += (s, e) =>
{
    Console.WriteLine("\n    Session stopped event.");
};
speechRecognizer.SpeechStartDetected += (s, e) =>
{
    Console.WriteLine("\n    SpeechStartDetected event.");
};
speechRecognizer.SpeechEndDetected += (s, e) =>
{
    Console.WriteLine("\n    SpeechEndDetected event.");
};
Console.WriteLine("Say something...");
speechRecognizer.StartContinuousRecognitionAsync().Wait();
Console.WriteLine("Press any key to stop");
Console.ReadKey();
speechRecognizer.StopContinuousRecognitionAsync().Wait();