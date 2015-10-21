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


        private static string[] FamilyNames = { "Максимов", "Андреев", "Сергеев", "Сидоров", "Иванов", "Петров", "Абрамов", "Евгеньев", "Архипов", "Антонов", "Дмитриев", "Леонидов", "Денисов", "Тарасов", "Владимиров", "Константинов", "Николаев", "Романов", "Константинов", "Артемьев", "Филиппов", "Викторов", "Васильев", "Прохоров", "Алексеев", "Михайлов", "Афанасьев", "Харитонов" };
        private static string[] GivenNames = { "Максим", "Андрей", "Сергей", "Сидор", "Иван", "Пётр", "Абрам", "Евгений", "Архип", "Антон", "Дмитрий", "Леонид", "Денис", "Тарас", "Владимир", "Константин", "Николай", "Роман", "Константин", "Артём", "Филипп", "Виктор", "Василий", "Прохор", "Алексей", "Михаил", "Афанасий", "Харитон" };
        private static string[] MiddleNames = { "Максимович", "Андреевич", "Сергеевич", "Сидорович", "Иванович", "Петрович", "Абрамович", "Евгеньевич", "Архипович", "Антонович", "Дмитриевич", "Леонидович", "Денисович", "Тарасович", "Владимирович", "Константинович", "Николаевич", "Романович", "Константинович", "Артёмович", "Филиппович", "Викторович", "Васильевич", "Прохорович", "Алексеевич", "Михайлович", "Афанасьевич", "Харитонович" };
        private static string[] Streets = { "Невский пр.", "ул.Оптиков", "ул.Фрунзе", "ул.Дыбенко", "Пискарёвский пр.", "ул. Таллинская", "ул. Казанская", "наб. канала Грибоедова", "пл. Труда" };

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
            return Streets[R.Next(Streets.Length)] + ", д." + R.Next(30).ToString() + ", кв." + R.Next(150).ToString();
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
                        Assigner = new ResourceReference() { Reference = "Link/" + Ids.organization },
                        Period = new Period
                        {
                             StartElement = new FhirDateTime("01.02.2012"),
                             EndElement = new FhirDateTime("01.02.2018")
                        } //для паспорта
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
                Subscriber = new ResourceReference { Reference = "Patient/" + patient },
                Identifier = new List<Identifier>
                {
                    new Identifier
                    {
                        System = "urn:oid:1.2.643.5.1.13.2.1.1.635.23607",//System = ...635.[код страховой компании]
                        Value = "1234567891011121",
                        //Period = new Period
                        //{
                        //     StartElement = new FhirDateTime("01.02.2012"),
                        //     EndElement = new FhirDateTime("01.02.2018")
                        //}
                    }
                }
            };
        }

        public Bundle SetBundleOrder(Order order, DiagnosticOrder diagnosticOrder,
                                     Specimen specimen, Encounter encounter, Condition condition,
                                     Observation observation, Practitioner practitioner, Coverage coverage)
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
            return bundle;
        }

        //продумать ссылки!!

        //Practitioner/131d7d5d-0f21-451d-86ec-27fa3e069e1a
        //519a08f4-c240-4e58-aa66-fe2a017b8d94 
        public Order SetOrder(string patient, string practitioner)
        {
            return new Order
            {
                Identifier = new List<Identifier>
                {
                    new Identifier
                    {
                        System = "urn:oid:1.2.643.2.69.1.2.6",
                        Value = "IdOrderMis" + new Random().Next(1000)
                    }
                },
                Date = "01.01.2012",
                Subject = new ResourceReference { Reference = "Patient/" + patient },
                Source = new ResourceReference { Reference = practitioner },
                Target = new ResourceReference { Reference = "Organization/" + Ids.organization },
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

        private OrderResponse SetOrderResponse()
        {
            return new OrderResponse
            {
                Identifier = new List<Identifier>
                {
                    new Identifier
                    {
                        System = "urn:oid:1.2.643.2.69.1.2.2",
                        Value = "IdOrderLis" + new Random().Next(100)
                    }
                },
                Request = new ResourceReference { Reference = "Order/" + Ids.order },
                Date = "02.01.2012",
                Who = new ResourceReference { Reference = "Organization/" + Ids.organization },
                OrderStatus_ = OrderResponse.OrderStatus.Completed,
                Description = "Комментарий к заказу",
                Fulfillment = new List<ResourceReference>() { new ResourceReference { Reference = Ids.diagnosticReport } }
            };
        }

        public DiagnosticOrder SetDiagnosticOrder(string patient, string practitioner, string specimen, string supportInfo)
        {
            return new DiagnosticOrder
            {
                //Id самого Ордера
                Id = Ids.diagnosticReport,
                //Id пациента, генерится при добавлении поциента, иначе пеняй на себя
                Subject = new ResourceReference { Reference = "Patient/" + patient },
                //Id доктора, доктор добавляется в бандле Id из Ids
                Orderer = new ResourceReference { Reference = practitioner },
                //Id Encounter, он тоже в бандле
                Encounter = new ResourceReference { Reference = Ids.encounter },
                //Пока тут Id Condition
                SupportingInformation = new List<ResourceReference> { new ResourceReference { Reference = supportInfo } },
                Specimen = new List<ResourceReference> { new ResourceReference { Reference = specimen } },
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
        }

        public Specimen SetSpecimen(string patient)
        {
            return new Specimen
            {
                Id = Ids.specimen,
                Type = new CodeableConcept
                {
                    Coding = new List<Coding> { new Coding { System = Dictionary.TYPE_SPECIMEN, Code = "1", Version = "1" } }
                },
                Subject = new ResourceReference { Reference = "Patient/" + patient },
                Collection = new Specimen.SpecimenCollectionComponent
                {
                    Comment = new List<string> { "Комментарий к биоматериалу" },
                    Collected = new Hl7.Fhir.Model.FhirDateTime(1998)
                },
                Container = new List<Specimen.SpecimenContainerComponent>
                {
                    new Specimen.SpecimenContainerComponent
                    {
                        Identifier = new List<Identifier>() { new Identifier { System = "http://netrika.ru/container-type-identifier", Value = "barCode111" }}, // System?
                        Type = new CodeableConcept
                        {
                            Coding = new List<Coding>() { new Coding { System = Dictionary.TYPE_CONTAINER, Code = "1", Version = "1" } }
                        }
                    }
                }
            };
        }

        public Encounter SetEncounter(string patient, string[] condition)
        {
            return new Encounter
            {
                Id = Ids.encounter,
                Identifier = new List<Identifier> { new Identifier { System = "urn:oid:1.2.643.2.69.1.2.6", Value = "IdCaseMis" } },
                Status = Encounter.EncounterState.InProgress,
                Class = Encounter.EncounterClass.Ambulatory,
                Type = new List<CodeableConcept>
                {
                    new CodeableConcept
                    {
                       Coding = new List<Coding> { new Coding { System = Dictionary.TYPE_CASE, Code = "2", Version = "1" } }
                    }
                },
                Patient = new ResourceReference { Reference = "Patient/" + patient },
                Reason = new List<CodeableConcept>()
                {
                    new CodeableConcept
                    {
                        Coding = new List<Coding> { new Coding { System = Dictionary.REASON, Code = "1", Version = "1" } }
                    }
                },
                Indication = new List<ResourceReference> { new ResourceReference { Reference = condition[0] } },
                ServiceProvider = new ResourceReference { Reference = "Organization/" + Ids.organization }
            };
        }

        public Condition SetCondition(string patient)
        {
            return new Condition
            {
                Id = Ids.condition,
                Identifier = new List<Identifier>()
                {
                    new Identifier
                    {
                        System = "urn:oid:1.2.643.2.69.1.1.1.61",
                        Value = "Стандарт первичной медико-санитарной помощи при хронической болезни почек 4 стадии"
                    }
                },
                //Генерится при создании
                Patient = new ResourceReference { Reference = "Patient/" + patient },
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
                Notes = "Уточнение",
                //dueTo = new DueTo // Сопутствующее заболевание/осложнение 
                //{
                //    target = new Condition
                //    {
                //        identifier = new Identifier[]
                //        {
                //            new Identifier{
                //            System = "urn:oid:1.2.643.2.69.1.1.1.61",}
                //            //value?
                //        },
                //        subject = new ResourceReference { reference = "Patient/" + patient },
                //        dateAsserted = Convert.ToDateTime("01.02.2012"),
                //        Code = new CodeableConcept
                //        {
                //            coding = new Coding[] { new Coding { System = Dictionary.DIAGNOSIS, Code = "N18.9", Version = 1 } }
                //        },
                //        category = new CodeableConcept
                //        {
                //            coding = new Coding[] { new Coding { System = Dictionary.TYPE_CONDITION, Code = "diagnosis", Version = 1 } }
                //        },
                //        clinicalStatus = "confirmed",
                //        notes = "Уточнение",
                //    }
                //}
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
                    Units = "ммоль/л"
                }
            };
        }

        public Practitioner SetPractitioner()
        {
            return new Practitioner
            {
                Id = Ids.partitioner,
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
                        ManagingOrganization = new ResourceReference { Reference = "Organization/" + Ids.organization },
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

        public Bundle SetBundleResult(string patient)
        {
            return new Bundle
            {
                Meta = new Meta() { Profile = new string[] { MetaBundleResult } },
                Entry = new List<Bundle.BundleEntryComponent>()
                {
                    new Bundle.BundleEntryComponent()
                    {
                        Resource = SetOrderResponse(),
                        Transaction = new Bundle.BundleEntryTransactionComponent { Method  = Bundle.HTTPVerb.POST, Url  = "OrderResponse"}
                    },
                    new Bundle.BundleEntryComponent()
                    {
                        Resource =  SetDiagnosticReport(patient),
                        Transaction = new Bundle.BundleEntryTransactionComponent() {  Method  = Bundle.HTTPVerb.POST, Url  = "DiagnosticReport"}
                    },

                    new Bundle.BundleEntryComponent() 
                    {
                        Resource = SetObservation_BundleResult(),
                        Transaction = new Bundle.BundleEntryTransactionComponent() { Method  = Bundle.HTTPVerb.POST, Url  = "Observation"}
                    },
                    new Bundle.BundleEntryComponent()
                    {
                        Resource =  SetPractitioner(),
                        Transaction = new Bundle.BundleEntryTransactionComponent() {  Method  = Bundle.HTTPVerb.POST, Url   = "Practitioner"}
                    },

                },

            };
        }

        private DiagnosticReport SetDiagnosticReport(string patient)
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
                Subject = new ResourceReference { Reference = "Patient/" + patient },
                Performer = new ResourceReference { Reference = Ids.partitioner },
                RequestDetail = new List<ResourceReference> { new ResourceReference() { Reference = "DiagnosticOrder/" + Ids.diagnosticOrder } },
                Result = new List<ResourceReference>() { new ResourceReference { Reference = Ids.observation } },
                Conclusion = "Текст заключения по услуге B03.016.006",
                PresentedForm = new List<Attachment>
                {
                    new Attachment
                    {
                         Data = Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(qwer)))
                    }
                }
                //presentedForm ?? 
            };
        }

        private Observation SetObservation_BundleResult()
        {
            return new Observation
            {
                Id = Ids.observation,
                Code = new CodeableConcept
                {
                    Coding = new List<Coding> { new Coding { System = Dictionary.CODE_TEST, Code = "17861-6", Version = "1" } }
                },
                Comments = "Комментарий к результату теста",
                Issued = Convert.ToDateTime("02.02.2012"),
                Status = Observation.ObservationStatus.Final,
                Method = new CodeableConcept
                {
                    Coding = new List<Coding> { new Coding { System = "urn:oid:1.2.643.2.69.1.2.2", Code = "Химический", Version = "1" } }
                },
                Performer = new List<ResourceReference>() { new ResourceReference() { Reference = Ids.partitioner } },


                // или value[x]
                DataAbsentReason = new CodeableConcept
                {
                    Coding = new List<Coding>() { new Coding { System = "urn:oid:1.2.643.2.69.1.1.1.38", Code = "1", Version = "1" } }

                }, // Code?

                ReferenceRange = new List<Observation.ObservationReferenceRangeComponent>()
                {
                    new Observation.ObservationReferenceRangeComponent()
                    {
                        Low = new Quantity
                        {
                            Value = 2.15m,
                            Units = "ммоль/л"
                        },
                        High = new Quantity
                        {
                            Value = 2.5m,
                            Units = "ммоль/л"
                        }
                    }
                }

            };
        }
    }
}
