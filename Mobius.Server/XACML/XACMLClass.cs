using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Lagash.Xacml.Core.Policy;
using Lagash.Xacml.ControlCenter.TreeNodes;
using Lagash.Xacml.Core;
using System.Xml;
using System.IO;

namespace FirstGenesis.Mobius.Server.MobiusHISEService.XACML
{
    public class XACMLClass
    {


        public XACMLClass()
        {

        }
        public string RuleStartDate
        {
            get;
            set;
        }

        public string RuleEndDate
        {
            get;
            set;
        }

        public List<string> Subject
        {
            get;
            set;
        }

        public string PurposeofUse
        {
            get;
            set;
        }

        public byte[] CreateXACMLPolicy(string strPolicyId, string strPolicyDescription, string strXPathVersion, string strRuleId,
           string strRuleDescription,List<string> strSubjectContents, string strIssuer, string strSubjectCategory, List<string> strResourceContents,
           string strActionContents, string strFileNameWithPath, string strRuleStartDate, string strRuleEndDate)
        {

            // Creates New Policy Element
            PolicyElementReadWrite objPolicyElementReadWrite = new PolicyElementReadWrite(strPolicyId, strPolicyDescription, null, new RuleCollectionReadWrite(), Lagash.Xacml.PolicySchema1.RuleCombiningAlgorithms.DenyOverrides, new ObligationCollectionReadWrite(), strXPathVersion, null, null, null, Lagash.Xacml.Core.XacmlVersion.Version11); //TODO: check version            

            // Creates New Rule Element
            RuleElementReadWrite objRuleElementReadWrite = new RuleElementReadWrite(strRuleId, strRuleDescription, null, null, Effect.Permit, Lagash.Xacml.Core.XacmlVersion.Version11);

            // Create Target Element
            TargetElementReadWrite objTargetElementReadWrite = new TargetElementReadWrite(new ResourcesElementReadWrite(true, new TargetItemCollectionReadWrite(), Lagash.Xacml.Core.XacmlVersion.Version11),
                new SubjectsElementReadWrite(true, new TargetItemCollectionReadWrite(), Lagash.Xacml.Core.XacmlVersion.Version11),
                new ActionsElementReadWrite(true, new TargetItemCollectionReadWrite(), Lagash.Xacml.Core.XacmlVersion.Version11),
                new EnvironmentsElementReadWrite(true, new TargetItemCollectionReadWrite(),
                    Lagash.Xacml.Core.XacmlVersion.Version11), Lagash.Xacml.Core.XacmlVersion.Version11);

            Target objtargetNode = new Target(objTargetElementReadWrite);
            // Create Target Item(Subject)
            foreach (var tempSubject in strSubjectContents)
            {
                TargetMatchCollectionReadWrite subject = new TargetMatchCollectionReadWrite();
                subject.Add(
                    new SubjectMatchElementReadWrite(
                    Lagash.Xacml.PolicySchema1.InternalFunctions.StringEqual,
                    new AttributeValueElementReadWrite(Lagash.Xacml.PolicySchema1.InternalDataTypes.XsdString, tempSubject, Lagash.Xacml.Core.XacmlVersion.Version11),  //TODO: check version
                    new SubjectAttributeDesignatorElement(Lagash.Xacml.PolicySchema1.InternalDataTypes.XsdString, false, Lagash.Xacml.PolicySchema1.SubjectElement.ActionSubjectId, strIssuer, strSubjectCategory, Lagash.Xacml.Core.XacmlVersion.Version11), Lagash.Xacml.Core.XacmlVersion.Version11));  //TODO: check version
                SubjectElementReadWrite targetItemSubject = new SubjectElementReadWrite(subject, Lagash.Xacml.Core.XacmlVersion.Version11);  //TODO: check version

                TargetItem targetItemNode = new TargetItem(targetItemSubject);

                // adds subject to the target node
                objtargetNode.Nodes.Add(targetItemNode);
                objtargetNode.TargetDefinition.Subjects.IsAny = false;
                objtargetNode.TargetDefinition.Subjects.ItemsList.Add(targetItemSubject);
            }


            //create target item(resource)
            foreach (var tempResoruce in strResourceContents)
            {
                TargetMatchCollectionReadWrite resource = new TargetMatchCollectionReadWrite();
                resource.Add(
                    new ResourceMatchElementReadWrite(
                    Lagash.Xacml.PolicySchema1.InternalFunctions.StringEqual,
                    new AttributeValueElementReadWrite(Lagash.Xacml.PolicySchema1.InternalDataTypes.XsdString, tempResoruce, Lagash.Xacml.Core.XacmlVersion.Version11),  //TODO: check version
                    new ResourceAttributeDesignatorElement(Lagash.Xacml.PolicySchema1.InternalDataTypes.XsdString, false, Lagash.Xacml.PolicySchema1.ResourceElement.ResourceId, strIssuer, Lagash.Xacml.Core.XacmlVersion.Version11), Lagash.Xacml.Core.XacmlVersion.Version11)); //TODO: check version
                ResourceElementReadWrite targetItemResource = new ResourceElementReadWrite(resource, Lagash.Xacml.Core.XacmlVersion.Version11); //TODO: check version

                TargetItem targetItemNode1 = new TargetItem(targetItemResource);

                objtargetNode.Nodes.Add(targetItemNode1);
                objtargetNode.TargetDefinition.Resources.IsAny = false;
                objtargetNode.TargetDefinition.Resources.ItemsList.Add(targetItemResource);
            }


            //create target item(action)
            TargetMatchCollectionReadWrite action = new TargetMatchCollectionReadWrite();
            action.Add(
                new ActionMatchElementReadWrite(
                Lagash.Xacml.PolicySchema1.InternalFunctions.StringEqual,
                new AttributeValueElementReadWrite(Lagash.Xacml.PolicySchema1.InternalDataTypes.XsdString, strActionContents, Lagash.Xacml.Core.XacmlVersion.Version11),  //TODO: check version
                new ActionAttributeDesignatorElement(Lagash.Xacml.PolicySchema1.InternalDataTypes.XsdString, false, Lagash.Xacml.PolicySchema1.ActionElement.ActionId, strIssuer, Lagash.Xacml.Core.XacmlVersion.Version11), Lagash.Xacml.Core.XacmlVersion.Version11)); //TODO: check version
            ActionElementReadWrite targetItemAction = new ActionElementReadWrite(action, Lagash.Xacml.Core.XacmlVersion.Version11); //TODO: check version

            TargetItem actionNode = new TargetItem(targetItemAction);

            objtargetNode.Nodes.Add(actionNode);
            objtargetNode.TargetDefinition.Actions.IsAny = false;
            objtargetNode.TargetDefinition.Actions.ItemsList.Add(targetItemAction);

            // Adds target to the rule
            objRuleElementReadWrite.Target = objTargetElementReadWrite;

            // Create new condition element(greater than equal to)
            ConditionElementReadWrite objConditionElementReadWrite = new ConditionElementReadWrite(Lagash.Xacml.PolicySchema1.InternalFunctions.And, new IExpressionCollectionReadWrite(), XacmlVersion.Version11);

            ApplyElement objApplyElement1 = new ApplyElement(Lagash.Xacml.PolicySchema1.InternalFunctions.DateGreaterThanOrEqual, new IExpressionCollectionReadWrite(), Lagash.Xacml.Core.XacmlVersion.Version11);
            FunctionExecution objFunctionExecution1 = new FunctionExecution(objApplyElement1);

            ApplyElement objApplyElement2 = new ApplyElement(Lagash.Xacml.PolicySchema1.InternalFunctions.DateOneAndOnly, new IExpressionCollectionReadWrite(), Lagash.Xacml.Core.XacmlVersion.Version11);
            FunctionExecution objFunctionExecution2 = new FunctionExecution(objApplyElement2);

            AttributeSelectorElement objAttributeSelectorElement = new AttributeSelectorElement(Lagash.Xacml.PolicySchema1.InternalDataTypes.XsdDate, true, null, XacmlVersion.Version11);
            objApplyElement2.Arguments.Add(objAttributeSelectorElement);

            objApplyElement1.Arguments.Add(objApplyElement2);

            AttributeValueElementReadWrite objAttributeValueElementReadWrite = new AttributeValueElementReadWrite(Lagash.Xacml.PolicySchema1.InternalDataTypes.XsdDate, strRuleStartDate, XacmlVersion.Version11);

            AttributeValue objAttributeValue = new AttributeValue(objAttributeValueElementReadWrite);

            objApplyElement1.Arguments.Add(objAttributeValueElementReadWrite);


            // Create new condition element(less than equal to)            

            ApplyElement objApplyElementless1 = new ApplyElement(Lagash.Xacml.PolicySchema1.InternalFunctions.DateLessThanOrEqual, new IExpressionCollectionReadWrite(), Lagash.Xacml.Core.XacmlVersion.Version11);
            FunctionExecution objFunctionExecutionless1 = new FunctionExecution(objApplyElementless1);

            ApplyElement objApplyElementless2 = new ApplyElement(Lagash.Xacml.PolicySchema1.InternalFunctions.DateOneAndOnly, new IExpressionCollectionReadWrite(), Lagash.Xacml.Core.XacmlVersion.Version11);
            FunctionExecution objFunctionExecutionless2 = new FunctionExecution(objApplyElementless2);

            AttributeSelectorElement objAttributeSelectorElementless = new AttributeSelectorElement(Lagash.Xacml.PolicySchema1.InternalDataTypes.XsdDate, true, null, XacmlVersion.Version11);
            objApplyElementless2.Arguments.Add(objAttributeSelectorElementless);

            objApplyElementless1.Arguments.Add(objApplyElementless2);

            AttributeValueElementReadWrite objAttributeValueElementReadWriteless = new AttributeValueElementReadWrite(Lagash.Xacml.PolicySchema1.InternalDataTypes.XsdDate, strRuleEndDate, XacmlVersion.Version11);

            //AttributeValue objAttributeValue = new AttributeValue(objAttributeValueElementReadWriteless);

            objApplyElementless1.Arguments.Add(objAttributeValueElementReadWriteless);


            Condition objCondition = new Condition(objConditionElementReadWrite);

            objCondition.Nodes.Add(objFunctionExecution1);
            objCondition.ConditionDefinition.Arguments.Add(objApplyElement1);

            objCondition.Nodes.Add(objFunctionExecutionless1);
            objCondition.ConditionDefinition.Arguments.Add(objApplyElementless1);


            // Adds condition to the rule
            objRuleElementReadWrite.Condition = objConditionElementReadWrite;

            // Adds rule to the policy
            objPolicyElementReadWrite.Rules.Add(objRuleElementReadWrite);

            //Added on 04/16/2012 for returning byte data 
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            //Ends Addition

            //Commented on 04/16/2012 for returning byte data 
            XmlTextWriter writer = new XmlTextWriter(strFileNameWithPath, System.Text.Encoding.UTF8);
            //Ends commenting

            //Modified on 04/16/2012 for returning byte data 
            //XmlTextWriter writer = new XmlTextWriter(memoryStream, System.Text.Encoding.UTF8);
            //Ends Modification

            writer.Namespaces = true;
            writer.Formatting = Formatting.Indented;


            PolicyDocumentReadWrite objPolicyDocumentReadWrite = new PolicyDocumentReadWrite(Lagash.Xacml.Core.XacmlVersion.Version11);
            objPolicyDocumentReadWrite.Policy = objPolicyElementReadWrite;


            objPolicyDocumentReadWrite.WriteDocument(writer);

            //Added on 04/16/2012 for returning byte data 
            //String XmlizedString = null;
            //System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(this.GetType());
            //xs.Serialize(writer, this);
            //memoryStream = (System.IO.MemoryStream)writer.BaseStream;
            //XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
            //Ends Addition

            writer.Close();
            //Commented on 04/16/2012 for returning byte data 
            //return true;
            //Ends Comment

            //Added for fetching the byte data from temp location
            byte[] XACMLbyteData=null;
            FileStream fs = new FileStream(strFileNameWithPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(strFileNameWithPath).Length;
            XACMLbyteData = br.ReadBytes((int)numBytes);
            fs.Close();
            return XACMLbyteData;
        }
        private static String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

    }



}
