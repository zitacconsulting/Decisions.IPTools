using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using DecisionsFramework.Design.Flow;
using DecisionsFramework.Design.Properties;
using DecisionsFramework.Design.Flow.Mapping;
using DecisionsFramework.Design.Flow.CoreSteps;
using DecisionsFramework.Design.ConfigurationStorage.Attributes;
using DecisionsFramework.Design.Flow.Mapping.InputImpl;
namespace Zitac.IPTools;


[AutoRegisterStep("IP is in use", "Integration", "IPTools")]
[Writable]
public class IPIsInUse : BaseFlowAwareStep, ISyncStep, IDataConsumer, IDataProducer
{

    public DataDescription[] InputData
    {
        get
        {

            List<DataDescription> dataDescriptionList = new List<DataDescription>();
            dataDescriptionList.Add(new DataDescription((DecisionsType)new DecisionsNativeType(typeof(String)), "IP Address"));
            return dataDescriptionList.ToArray();
        }
    }

    public override OutcomeScenarioData[] OutcomeScenarios
    {
        get
        {
            List<OutcomeScenarioData> outcomeScenarioDataList = new List<OutcomeScenarioData>();

            outcomeScenarioDataList.Add(new OutcomeScenarioData("True"));
            outcomeScenarioDataList.Add(new OutcomeScenarioData("False"));
            outcomeScenarioDataList.Add(new OutcomeScenarioData("Error", new DataDescription(typeof(string), "Error Message")));
            return outcomeScenarioDataList.ToArray();
        }
    }

    public ResultData Run(StepStartData data)
    {
        string? ipAddress = data.Data["IP Address"] as string;

        try
        {
            using (Ping ping = new Ping())
            {
                // Send ping
                PingReply reply = ping.Send(ipAddress, 1000); // 1000ms timeout

                // Check if the remote machine replied to the ping
                bool Result = reply.Status == IPStatus.Success;
                if (reply.Status == IPStatus.Success)
                {
                    return new ResultData("True");
                }
                else
                {
                    return new ResultData("False");
                }

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("Result", (object)Result);
                return new ResultData("Done", (IDictionary<string, object>)dictionary);
            }
        }

        catch (Exception e)
        {
            string ExceptionMessage = e.ToString();
            return new ResultData("Error", (IDictionary<string, object>)new Dictionary<string, object>()
                {
                {
                    "Error Message",
                    (object) ExceptionMessage
                }
                });
        }



    }
}
