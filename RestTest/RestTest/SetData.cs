using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace RestTest
{
    class SetData
    {
        private const string MetaBundleOrder = "StructureDefinition/cd45a667-bde0-490f-b602-8d780acf4aa2";
        private const string MetaBundleResult = "StructureDefinition/21f687dd-0b3b-4a7b-af8f-04be625c0201";

        //private string refDiagnosticReport = "143e62fc-eee7-4273-899c-23c60c72cb1a";
        //private string refEncounter = "f0ceca14-6847-4ea4-b128-7c86820da428";

        //private string organization = "4a94e705-ee3e-46fc-bba0-0298e0fd5bd2";


        private static string[] FamilyNames = { "��������", "�������", "�������", "�������", "������", "������", "�������", "��������", "�������", "�������", "��������", "��������", "�������", "�������", "����������", "������������", "��������", "�������", "������������", "��������", "��������", "��������", "��������", "��������", "��������", "��������", "���������", "���������" };
        private static string[] GivenNames = { "������", "������", "������", "�����", "����", "ϸ��", "�����", "�������", "�����", "�����", "�������", "������", "�����", "�����", "��������", "����������", "�������", "�����", "����������", "����", "������", "������", "�������", "������", "�������", "������", "��������", "�������" };
        private static string[] MiddleNames = { "����������", "���������", "���������", "���������", "��������", "��������", "���������", "����������", "���������", "���������", "����������", "����������", "���������", "���������", "������������", "��������������", "����������", "���������", "��������������", "��������", "����������", "����������", "����������", "����������", "����������", "����������", "�����������", "�����������" };
        private static string[] Streets = { "������� ��.", "��.�������", "��.������", "��.�������", "����������� ��.", "��. ����������", "��. ���������", "���. ������ ����������", "��. �����" };

        static Random R = new Random((int)DateTime.Now.Ticks);
        private static string[] RandomFIO()
        {
            int len = new[] { FamilyNames, GivenNames, MiddleNames }.Min(x => x.Length);
            return new string[] { FamilyNames[R.Next(len)], GivenNames[R.Next(len)], MiddleNames[R.Next(len)] };
        }

        private static string RandomBirthDate()
        {
            return new DateTime(1950, 1, 1).AddDays(R.Next((DateTime.Today.AddYears(-30) - new DateTime(1950, 1, 1)).Days)).ToString("dd.MM.yyyy");
        }

        private static string RandomAddress()
        {
            return Streets[R.Next(Streets.Length)] + ", �." + R.Next(30).ToString() + ", ��." + R.Next(150).ToString();
        }

        public Patient SetPatient()
        {
            
            return new Patient
            {
                Address = new List<Address>
                {
                    new Address() { Use = Address.AddressUse.Home, Text = RandomAddress() }
                },
                Identifier = new List<Identifier>
                {
                    new Identifier()
                    {
                        System = "urn:oid:1.2.643.5.1.34",
                        Value = "IdPatientMis" + DateTime.Now,
                        Assigner = new ResourceReference() { Reference = References.organization_Patient },
                        Period = new Period
                        {
                             StartElement = new FhirDateTime("01.02.2012"),
                             EndElement = new FhirDateTime("01.02.2018")
                        } //��� ��������
                    }
                },
                BirthDate = RandomBirthDate(),
                Gender = AdministrativeGender.Male,
                Name = new List<HumanName>
                {
                    new HumanName
                    {
                        Family = new List<string> { RandomFIO()[0] },
                        Given = new List<string> { RandomFIO()[1], RandomFIO()[2] }
                    }
                }
            };
        }

        public Coverage SetCoverage(string patient)
        {
            return new Coverage
            {
                Id = Ids.coverage,
                Type = new Coding { System = Dictionary.TYPE_COVERAGE, Code = "2", Version = "1" },
                Subscriber = new ResourceReference { Reference = patient },
                Identifier = new List<Identifier>
                {
                    new Identifier
                    {
                        System = "urn:oid:1.2.643.5.1.13.2.1.1.635.23607",//System = ...635.[��� ��������� ��������]
                        Value = "1234567891011121",
                        Period = new Period
                        {
                             StartElement = new FhirDateTime("01.02.2012"),
                             EndElement = new FhirDateTime("01.02.2018")
                        }
                    }
                }
            };
        }

        public Bundle SetBundleOrder(Order order, DiagnosticOrder diagnosticOrder,
                                     Specimen specimen, Encounter encounter, Condition condition,
                                     Observation observation, Practitioner practitioner, Coverage coverage, Patient patient)
        {
            Bundle bundle = new Bundle();
            bundle.Meta = new Meta() { Profile = new string[] { MetaBundleOrder } };
            bundle.Entry = new List<Bundle.BundleEntryComponent>();

            if (order != null)
            {
                Bundle.BundleEntryComponent component = new Bundle.BundleEntryComponent
                {
                    Resource = order,
                    Transaction = new Bundle.BundleEntryTransactionComponent { Method = Bundle.HTTPVerb.POST, Url = "Order" }
                };
                bundle.Entry.Add(component);
            }

            if (diagnosticOrder != null)
            {
                Bundle.BundleEntryComponent component = new Bundle.BundleEntryComponent
                {
                    Resource = diagnosticOrder,
                    Transaction = new Bundle.BundleEntryTransactionComponent() { Method = Bundle.HTTPVerb.POST, Url = "DiagnosticOrder" }
                };
                bundle.Entry.Add(component);
            }

            if (specimen != null)
            {
                Bundle.BundleEntryComponent component = new Bundle.BundleEntryComponent
                {
                    Resource = specimen,
                    Transaction = new Bundle.BundleEntryTransactionComponent() { Method = Bundle.HTTPVerb.POST, Url = "Specimen" }
                };
                bundle.Entry.Add(component);
            }

            if (encounter != null)
            {
                Bundle.BundleEntryComponent component = new Bundle.BundleEntryComponent
                {
                    Resource = encounter,
                    Transaction = new Bundle.BundleEntryTransactionComponent() { Method = Bundle.HTTPVerb.POST, Url = "Encounter" }
                };
                bundle.Entry.Add(component);
            }

            if (condition != null)
            {
                Bundle.BundleEntryComponent component = new Bundle.BundleEntryComponent
                {
                    Resource = condition,
                    Transaction = new Bundle.BundleEntryTransactionComponent() { Method = Bundle.HTTPVerb.POST, Url = "Condition" }
                };
                bundle.Entry.Add(component);
            }

            if (observation != null)
            {
                Bundle.BundleEntryComponent component = new Bundle.BundleEntryComponent
                {
                    Resource = observation,
                    Transaction = new Bundle.BundleEntryTransactionComponent() { Method = Bundle.HTTPVerb.POST, Url = "Observation" }
                };
                bundle.Entry.Add(component);
            }

            if (practitioner != null)
            {
                Bundle.BundleEntryComponent component = new Bundle.BundleEntryComponent
                {
                    Resource = practitioner,
                    Transaction = new Bundle.BundleEntryTransactionComponent() { Method = Bundle.HTTPVerb.POST, Url = "Practitioner" }
                };
                bundle.Entry.Add(component);
            }
            if (coverage != null)
            {
                Bundle.BundleEntryComponent component = new Bundle.BundleEntryComponent
                {
                    Resource = coverage,
                    Transaction = new Bundle.BundleEntryTransactionComponent() { Method = Bundle.HTTPVerb.POST, Url = "Coverage" }
                };
                bundle.Entry.Add(component);
            }
            if (patient != null)
            {
                Bundle.BundleEntryComponent component = new Bundle.BundleEntryComponent
                {
                    Resource = patient,
                    Transaction = new Bundle.BundleEntryTransactionComponent() { Method = Bundle.HTTPVerb.POST, Url = "Patient" }
                };
                bundle.Entry.Add(component);
            }
            return bundle;
        }

        public Order SetOrder(string patient, string practitioner, string organization)
        {
            return new Order
            {
                Identifier = new List<Identifier>
                {
                    new Identifier
                    {
                        System = "urn:oid:1.2.643.2.69.1.2.6",
                        Value = "IdOrderMis" + DateTime.Now,
                        Assigner = new ResourceReference { Reference = organization }
                    }
                },
                Date = "01.01.2012",
                Subject = new ResourceReference { Reference = patient },
                Source = new ResourceReference { Reference = practitioner },
                Target = new ResourceReference { Reference = organization },
                //��� ������ ��������� Ids.diagnosticReport (�� ����������)
                Detail = new List<ResourceReference> { new ResourceReference { Reference = Ids.diagnosticReport } },
                When = new Order.OrderWhenComponent
                {
                    Code = new CodeableConcept
                    {
                        Coding = new List<Coding>
                        {
                            new Coding { System = Dictionary.PRIORITY_EXECUTION, Code = "Routine", Version = "1" }
                        }
                    }
                }
            };
        }

        public DiagnosticOrder SetDiagnosticOrder(string patient, string practitioner, string encounter, string specimen, string[] supportInfo)
        {
            DiagnosticOrder d = new DiagnosticOrder
            {
                Id = Ids.diagnosticReport,
                //Id ��������, ��������� ��� ���������� ��������, ����� ����� �� ����
                Subject = new ResourceReference { Reference = patient },
                //Id �������, ������ ����������� � ������ Id �� Ids
                Orderer = new ResourceReference { Reference = practitioner },
                Encounter = new ResourceReference { Reference = encounter },
                Status = DiagnosticOrder.DiagnosticOrderStatus.Requested,
                Item = new List<DiagnosticOrder.DiagnosticOrderItemComponent>
                {
                    
                    new DiagnosticOrder.DiagnosticOrderItemComponent
                    {
                        Code = new CodeableConcept
                        {
                            Extension = new List<Extension>
                            {
                                new Extension
                                {
                                    Url = Dictionary.EXTENSION,
                                    Value = new CodeableConcept
                                    {
                                         Coding = new List<Coding>
                                         {
                                              new Coding { System = Dictionary.EXTENSION_FINANCE, Code = "1", Version = "1" }
                                         }
                                    }
                                },
                                new Extension
                                {
                                    Url = "urn:oid:1.2.643.2.69.1.100.2",
                                    Value = new ResourceReference
                                    {
                                         Reference = "Coverage/f30481cf-6a5f-4614-bb29-11542b790900"
                                    }
                                }
                            
                            },
                            Coding = new List<Coding>
                            {
                               new Coding { System = Dictionary.CODE_SERVICE, Code = "B03.016.002", Version = "1"}
                            }
                        }
                    }
                }
            };

            //�������������� ���������
            if (specimen != null)
                d.Specimen = new List<ResourceReference> { new ResourceReference { Reference = specimen } };
            // Condition/Observation ������ �� Ids
            // ����� ������� condition_MIN �� ������ �� codition_min, � ������ ������ codition
            if (supportInfo != null)
            {
                d.SupportingInformation = new List<ResourceReference>();
                foreach (string s in supportInfo)
                    d.SupportingInformation.Add(new ResourceReference { Reference = s });
            }

            return d;
        }

        public Specimen SetSpecimen_Full(string patient)
        {
            return new Specimen
            {
                Id = Ids.specimen,
                Type = new CodeableConcept
                {
                    Coding = new List<Coding> { new Coding { System = Dictionary.TYPE_SPECIMEN, Code = "1", Version = "1" } }
                },
                Subject = new ResourceReference { Reference = patient },
                Collection = new Specimen.SpecimenCollectionComponent
                {
                    Comment = new List<string> { "����������� � ������������" },
                    Collected = new Hl7.Fhir.Model.FhirDateTime(1998, 10, 1)
                },
                Container = new List<Specimen.SpecimenContainerComponent>
                {
                    new Specimen.SpecimenContainerComponent
                    {
                        Identifier = new List<Identifier>() { new Identifier { System = "urn:oid:1.2.643.2.69.1.2.2", Value = "barCode111" }},
                        Type = new CodeableConcept
                        {
                            Coding = new List<Coding>() { new Coding { System = Dictionary.TYPE_CONTAINER, Code = "1", Version = "1" } }
                        }
                    }
                }
            };
        }

        public Specimen SetSpecimen_Min(string patient)
        {
            return new Specimen
            {
                Id = Ids.specimen,
                Subject = new ResourceReference { Reference = patient },
                Collection = new Specimen.SpecimenCollectionComponent { Collected = new Hl7.Fhir.Model.Date("27.05.2015 0:00:00") },
            };
        }

        //���� ��� condition ������ ������ ������ � �������
        public Encounter SetEncounter(string patient, string[] condition, string organization)
        {
            return new Encounter
            {
                Id = Ids.encounter,
                Identifier = new List<Identifier> { new Identifier { System = "urn:oid:1.2.643.2.69.1.2.6", Value = "IdCaseMis" + DateTime.Now } },
                Status = Encounter.EncounterState.InProgress,
                Class = Encounter.EncounterClass.Ambulatory,
                Type = new List<CodeableConcept>
                {
                    new CodeableConcept
                    {
                       Coding = new List<Coding> { new Coding { System = Dictionary.TYPE_CASE, Code = "2", Version = "1" } }
                    }
                },
                Patient = new ResourceReference { Reference = patient },
                Reason = new List<CodeableConcept>() //�������������� ��������
                {
                    new CodeableConcept
                    {
                        Coding = new List<Coding> { new Coding { System = Dictionary.REASON, Code = "1", Version = "1" } }
                    }
                },
                Indication = new List<ResourceReference> { new ResourceReference { Reference = condition[0] } },
                ServiceProvider = new ResourceReference { Reference = organization }
            };
        }

        public Condition SetCondition_Full(string patient)
        {
            return new Condition
            {
                Id = Ids.condition,
                Identifier = new List<Identifier>()
                {
                    new Identifier
                    {
                        System = "urn:oid:1.2.643.2.69.1.1.1.61",
                        Value = "�������� ��������� ������-���������� ������ ��� ����������� ������� ����� 4 ������"
                    }
                },
                //��������� ��� ��������
                Patient = new ResourceReference { Reference = patient },
                DateAsserted = "01.02.2012",
                Code = new CodeableConcept
                {
                    Coding = new List<Coding> { new Coding { System = Dictionary.DIAGNOSIS, Code = "N18.9", Version = "1" } }
                },
                Category = new CodeableConcept
                {
                    Coding = new List<Coding> { new Coding { System = Dictionary.TYPE_CONDITION, Code = "diagnosis", Version = "1" } }
                },
                ClinicalStatus = Condition.ConditionClinicalStatus.Confirmed,
                Notes = "���������",
                DueTo = new List<Condition.ConditionDueToComponent> // ������������� �����������/���������� 
                {   
                    new  Condition.ConditionDueToComponent
                    {
                          Target = new ResourceReference {Reference = Ids.condition_min}
                    }
                  
                }
            };
        }

        public Condition SetCondition_MinDiag(string patient)
        {
            return new Condition
            {
                Id = Ids.condition_min,
                //��������� ��� ��������
                Patient = new ResourceReference { Reference = patient },//R10.0 N18.9
                Code = new CodeableConcept
                {
                    Coding = new List<Coding> { new Coding { System = Dictionary.DIAGNOSIS, Code = "R10.0", Version = "1" } }
                },
                Category = new CodeableConcept
                {
                    Coding = new List<Coding> { new Coding { System = Dictionary.TYPE_CONDITION, Code = "diagnosis", Version = "1" } }
                },
                ClinicalStatus = Condition.ConditionClinicalStatus.Confirmed,
            };
        }

        public Condition SetCondition_MinMenPause(string patient)
        {
            return new Condition
            {
                Id = Ids.condition_min,
                //��������� ��� ��������
                Patient = new ResourceReference { Reference = patient },
                Code = new CodeableConcept
                {
                    Coding = new List<Coding> { new Coding { System = Dictionary.MENOPAUSE, Code = "1", Version = "1" } }
                },
                Category = new CodeableConcept
                {
                    Coding = new List<Coding> { new Coding { System = Dictionary.TYPE_CONDITION, Code = "finding", Version = "1" } }
                },
                ClinicalStatus = Condition.ConditionClinicalStatus.Confirmed,
            };
        }

        public Observation SetObservation_BundleOrder()
        {
            return new Observation
            {
                Id = Ids.observation,
                Code = new CodeableConcept
                {
                    Coding = new List<Coding> { new Coding { System = Dictionary.TYPE_OBSERVATION, Code = "2", Version = "1" } }
                },
                Status = Observation.ObservationStatus.Final,
                Value = new Quantity
                {
                    Value = 2.2m,
                    Units = "�����/�"
                }
            };
        }

        public Practitioner SetPractitioner()
        {
            return new Practitioner
            {
                Id = Ids.practitioner,
                Identifier = new List<Identifier>
                {
                    new Identifier
                    {
                        System = "urn:oid:1.2.643.2.69.1.2.6",
                        Value = "IdDoctorMIS" + DateTime.Now
                    }
                },
                Name = new HumanName()
                {
                    Family = new List<string> { RandomFIO()[0] },
                    Given = new List<string> { RandomFIO()[1], RandomFIO()[2] }
                },
                PractitionerRole = new List<Practitioner.PractitionerPractitionerRoleComponent>()
                {
                    new Practitioner.PractitionerPractitionerRoleComponent
                    {
                        //���� �� ����� ������ ����� c ������ organization
                        ManagingOrganization = new ResourceReference { Reference = References.organization },
                        Role = new CodeableConcept
                        {
                            Coding = new List<Coding>() { new Coding { System = Dictionary.ROLE_PRACTITIONER, Code = "73", Version = "1" } }
                        },
                        Specialty = new List<CodeableConcept>()
                        {
                            new CodeableConcept()
                            {
                            Coding = new List<Coding>() { new Coding { System = Dictionary.SPECIALITY_PRACTITIONER, Code = "27", Version = "1" } }

                            }
                        }
                    }
                }
            };
        }

        public Bundle SetBundleResult(OrderResponse orderResp, DiagnosticReport diagReport, Observation observ, Practitioner pract)
        {
            Bundle bundle = new Bundle();
            bundle.Meta = new Meta() { Profile = new string[] { MetaBundleResult } };
            bundle.Entry = new List<Bundle.BundleEntryComponent>()
            {
                 new Bundle.BundleEntryComponent()
                    {
                        Resource = orderResp,
                        Transaction = new Bundle.BundleEntryTransactionComponent { Method  = Bundle.HTTPVerb.POST, Url  = "OrderResponse"}
                    },
                    new Bundle.BundleEntryComponent()
                    {
                        Resource =  diagReport,
                        Transaction = new Bundle.BundleEntryTransactionComponent() {  Method  = Bundle.HTTPVerb.POST, Url  = "DiagnosticReport"}
                    },

                    new Bundle.BundleEntryComponent() 
                    {
                        Resource = observ,
                        Transaction = new Bundle.BundleEntryTransactionComponent() { Method  = Bundle.HTTPVerb.POST, Url  = "Observation"}
                    }
            };

            //�������������� ��������
            if (pract != null)
            {
                Bundle.BundleEntryComponent component = new Bundle.BundleEntryComponent
                {
                    Resource = pract,
                    Transaction = new Bundle.BundleEntryTransactionComponent { Method = Bundle.HTTPVerb.POST, Url = "Practitioner" }
                };
                bundle.Entry.Add(component);
            }

            return bundle;
        }

        public OrderResponse SetOrderResponse(string order, string organization)
        {
            return new OrderResponse
            {
                Identifier = new List<Identifier>
                {
                    new Identifier
                    {
                        System = "urn:oid:1.2.643.2.69.1.2.2",
                        Value = "IdOrderLis" + DateTime.Now
                    }
                },
                Request = new ResourceReference { Reference = order },
                Date = "02.01.2012",
                Who = new ResourceReference { Reference = organization },
                OrderStatus_ = OrderResponse.OrderStatus.Completed,
                Description = "����������� � ������", //��������������
                Fulfillment = new List<ResourceReference>() { new ResourceReference { Reference = Ids.diagnosticReport } }
            };
        }

        public DiagnosticReport SetDiagnosticReport(string patient, string pract, string diagOrder)
        {
            var s = new PresentedForm();
            s.data = "<Envelope xmlns=\"http://hl7.org/fhir\"><presentedForm>Hello world</presentedForm></Envelope>";
            s.public_key = "BiAAACMuAABNQUcxAAIAADASBgcqhQMCAiQABgcqhQMCAh4BDROPfmmOeOh86V7iCavC+cv0KOeVDng82TgmfadiLAemoTP96XedalAisjD8r+AoRjh6AVGvaDfAlkMizps19w==";
            s.hash = "rQHUm/Ux16qN7/OswKxSJ3W58JBcHcKbQ2xPEDfnBz8=";
            s.sign = "VKN61+xjzRselU2Irnzj7hop9S3cc09STuZP5hkioa33wN+PFvPsD5omFQSV7jF31LzYoMf+ceHYq5EyUTZFAQ==";
            var qwer = (new RestSharp.Serializers.JsonSerializer()).Serialize(s);

            return new DiagnosticReport
            {
                Id = Ids.diagnosticReport,
                Name = new CodeableConcept
                {
                    Coding = new List<Coding>() { new Coding { System = Dictionary.CODE_SERVICE, Code = "B03.016.006", Version = "1" } }
                },
                Status = DiagnosticReport.DiagnosticReportStatus.Final,
                Issued = "03.01.2012",
                Subject = new ResourceReference { Reference = patient }, //������ ������������ �������
                Performer = new ResourceReference { Reference = pract },
                RequestDetail = new List<ResourceReference> { new ResourceReference() { Reference = diagOrder } },
                Result = new List<ResourceReference>() { new ResourceReference { Reference = Ids.observation_res } },
                Conclusion = "����� ���������� �� ������ B03.016.006",
                PresentedForm = new List<Attachment>
                {
                    new Attachment
                    {
                         Data = Encoding.UTF8.GetBytes(qwer)
                    }
                }
            };
        }

        public Observation SetObservation_BundleResult_Reason(string practitioner)
        {
            return new Observation
            {
                Id = Ids.observation_res,
                Code = new CodeableConcept
                {
                    Coding = new List<Coding> { new Coding { System = Dictionary.CODE_TEST, Code = "17861-6", Version = "1" } }
                },
                Comments = "����������� � ���������� �����",//��������������
                Issued = Convert.ToDateTime("02.02.2012"),
                Status = Observation.ObservationStatus.Final,
                Method = new CodeableConcept //��������������
                {
                    Coding = new List<Coding> { new Coding { System = "urn:oid:1.2.643.2.69.1.2.2", Code = "����������", Version = "1" } }
                },
                Performer = new List<ResourceReference>() { new ResourceReference() { Reference = practitioner } },


                // ��� value[x]
                DataAbsentReason = new CodeableConcept
                {
                    Coding = new List<Coding>() { new Coding { System = Dictionary.REASON_ABSENT, Code = "1", Version = "1" } }

                }, // Code?

                ReferenceRange = new List<Observation.ObservationReferenceRangeComponent>()
                {
                    new Observation.ObservationReferenceRangeComponent()
                    {
                        Low = new Quantity
                        {
                            Value = 2.15m,
                            Units = "�����/�"
                        },
                        High = new Quantity
                        {
                            Value = 2.5m,
                            Units = "�����/�"
                        }
                    }
                }

            };
        }

        public Observation SetObservation_BundleResult_Value(string practitioner)
        {
            return new Observation
            {
                Id = Ids.observation_res,
                Code = new CodeableConcept
                {
                    Coding = new List<Coding> { new Coding { System = Dictionary.CODE_TEST, Code = "17861-6", Version = "1" } }
                },
                Comments = "����������� � ���������� �����",//��������������
                Issued = Convert.ToDateTime("02.02.2012"),
                Status = Observation.ObservationStatus.Final,
                Method = new CodeableConcept //��������������
                {
                    Coding = new List<Coding> { new Coding { System = "urn:oid:1.2.643.2.69.1.2.2", Code = "����������", Version = "1" } }
                },
                Performer = new List<ResourceReference>() { new ResourceReference() { Reference = practitioner } },


                // ��� ������ Value?
                Value = new Hl7.Fhir.Model.Quantity
                {
                    Value = new decimal(2.2),
                    Units = "�����/�"
                },
                // "������-������",

                ReferenceRange = new List<Observation.ObservationReferenceRangeComponent>()
                {
                    new Observation.ObservationReferenceRangeComponent()
                    {
                        Low = new Quantity
                        {
                            Value = 2.15m,
                            Units = "�����/�"
                        },
                        High = new Quantity
                        {
                            Value = 2.5m,
                            Units = "�����/�"
                        }
                    }
                }
            };
        }
    }
}
