using DecisionsFramework.Design.Flow;
using DecisionsFramework.Design.Flow.Mapping;
using DecisionsFramework.Design.Flow.CoreSteps;
using DecisionsFramework.Design.ConfigurationStorage.Attributes;
using System.Text.RegularExpressions;

namespace Zitac.IPTools;


[AutoRegisterStep("Validate and Format Mac Address", "Integration", "IPTools")]
[Writable]
public class ValidateAndFormatMacAddress : BaseFlowAwareStep, ISyncStep, IDataConsumer, IDataProducer
{

    public DataDescription[] InputData
    {
        get
        {

            List<DataDescription> dataDescriptionList = new List<DataDescription>();
            dataDescriptionList.Add(new DataDescription((DecisionsType)new DecisionsNativeType(typeof(String)), "Mac Address"));
            return dataDescriptionList.ToArray();
        }
    }

    public override OutcomeScenarioData[] OutcomeScenarios
    {
        get
        {
            List<OutcomeScenarioData> outcomeScenarioDataList = new List<OutcomeScenarioData>();

            outcomeScenarioDataList.Add(new OutcomeScenarioData("Valid", new DataDescription(typeof(string), "Formatted Mac")));
            outcomeScenarioDataList.Add(new OutcomeScenarioData("Invalid"));
            return outcomeScenarioDataList.ToArray();
        }
    }

    public ResultData Run(StepStartData data)
    {
        string? macAddress = data.Data["Mac Address"] as string;

        macAddress = macAddress.Trim();

        // Regular expression to validate MAC Address
        var macRegex = new Regex(@"^([0-9A-Fa-f]{2}[:\-]?){5}([0-9A-Fa-f]{2})$");

        // Check if input is valid MAC Address
        if (!macRegex.IsMatch(macAddress))
        {
            return new ResultData("Invalid");
        }

        // Remove any separators (: or -)
        string unseparatedMac = macAddress.Replace(":", "").Replace("-", "");

        // Insert '-' after every 2 characters
        string Result =  Regex.Replace(unseparatedMac, ".{2}", "$0-").TrimEnd('-');
        
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        dictionary.Add("Formatted Mac", (object)Result);
        return new ResultData("Valid", (IDictionary<string, object>)dictionary);

    }
}
