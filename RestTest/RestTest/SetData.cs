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

        //продумать ссылки!!

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
                        System = "urn:oid:1.2.643.2.69.1.2.6",
                        Value = "IdPatientMis" + new Random().Next(1000),
                        Assigner = new ResourceReference() { Reference = "Link/4bcbf113-f99c-41fa-a92d-43f5684fffc5" },
                        //period  = new Period(Convert.ToDateTime("01.02.2012"), Convert.ToDateTime("01.02.2018")) для паспорта
                    }
                },
                BirthDate = RandomBirthDate(), //???????
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
                // id
                Type = new Coding { System = Dictionary.TYPE_COVERAGE, Code = "2", Version = "1" },
                Subscriber = new ResourceReference { Reference = "Patient/" + patient }, // для примера patient = 106043a2-6600-4590-bedd-6e26c76a6fed
                Identifier = new List<Identifier>
                {
                    new Identifier
                    {
                        System = "urn:oid:1.2.643.5.1.13.2.1.1.635.23607",//System = ...635.[код страховой компании]
                        Value = "1234567891011121",
                        //period = new Period(Convert.ToDateTime("01.02.2012"), Convert.ToDateTime("01.02.2018"))
                    }
                }
            };
        }

        public Bundle SetBundleOrder(string patient)
        {
            //BundleOrder b = new BundleOrder(SetOrder(patient), new Practitioner[] { SetPractitioner() },  new DiagnosticOrder[] { SetDiagnosticOrder(patient) },
            //   new Specimen[] { SetSpecimen(patient) }, SetEncounter(patient), new Condition[] { SetCondition(patient) },
            //     new Observation[] { SetObservation_BundleOrder() }, new Coverage[] { SetCoverage(patient) });
            return new Bundle
            {
                Meta = new Meta() { Profile = new string[] { "StructureDefinition/21f687dd-0b3b-4a7b-af8f-04be625c0201" } },
                Entry = new List<Bundle.BundleEntryComponent>()
                {
                    new Bundle.BundleEntryComponent()
                    {
                        Resource = SetOrder(patient),
                        //new BundleOrder{ order =  SetOrder(patient) },
                        Transaction = new Bundle.BundleEntryTransactionComponent { Method  = Bundle.HTTPVerb.POST, Url  = "Order"}
                    },
                    //new Entry
                    //{
                    //    resource =  SetDiagnosticOrder(patient),
                    //    //new BundleOrder { diagnosticOrder = new DiagnosticOrder[] { SetDiagnosticOrder(patient) } },
                    //    transaction = new Transaction { method  = "POST", url  = "DiagnosticOrder"}
                    //},
                    //new Entry
                    //{
                    //    resource = SetSpecimen(patient),
                    //    //new BundleOrder { specimen = new Specimen[] { SetSpecimen(patient) } },
                    //    transaction = new Transaction { method  = "POST", url  = "Specimen"}
                    //},
                    new Bundle.BundleEntryComponent() 
                    {
                        Resource = SetEncounter(patient),
                        //new BundleOrder { encounter =  SetEncounter(patient) },
                        Transaction = new Bundle.BundleEntryTransactionComponent() { Method  = Bundle.HTTPVerb.POST, Url  = "Encounter"}
                    },
                    //new Entry
                    //{
                    //    resource = SetCondition(patient),
                    //    //new BundleOrder { condition = new Condition[] { SetCondition(patient) } },
                    //    transaction = new Transaction { method  = "POST", url  = "Condition"}
                    //},
                    //new Entry
                    //{
                    //    resource = SetObservation_BundleOrder(),
                    //    //new BundleOrder { observation = new Observation[] { SetObservation_BundleOrder() } },
                    //    transaction = new Transaction { method  = "POST", url  = "Observation"}
                    //},
                    //new Entry
                    //{
                    //    resource = SetPractitioner(),
                    //    //new BundleOrder { practitioner = new Practitioner[] { SetPractitioner() } },
                    //    transaction = new Transaction { method  = "POST", url  = "Practitioner"}
                    //},
               //     new Entry
               //     {
               //         resource = SetCoverage(patient),
               ////         new BundleOrder { coverage = new Coverage[] { SetCoverage(patient)} },
               //         transaction = new Transaction { method  = "POST", url  = "Coverage"}
               //     },
                },
                //order = SetOrder(patient),
                //diagnosticOrder = new DiagnosticOrder[] { SetDiagnosticOrder(patient) },
                //specimen = new Specimen[] { SetSpecimen(patient) },
                //encounter = SetEncounter(patient),
                //condition = new Condition[] { SetCondition(patient) },
                //observation = new Observation[] { SetObservation_BundleOrder() },
                //practitioner = new Practitioner[] { SetPractitioner() },
                //coverage = new Coverage[] { SetCoverage(patient) }
            };
        }
        private Order SetOrder(string patient)
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
                Subject = new ResourceReference { Reference = "Patient/" + patient }, // для примера patient = 106043a2-6600-4590-bedd-6e26c76a6fed
                Source = new ResourceReference { Reference = "519a08f4-c240-4e58-aa66-fe2a017b8d94" },
                Target = new ResourceReference { Reference = "Organization/4bcbf113-f99c-41fa-a92d-43f5684fffc5" },
                Detail = new List<ResourceReference> { new ResourceReference { Reference = "143e62fc-eee7-4273-899c-23c60c72cb1a" } },
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

        private DiagnosticOrder SetDiagnosticOrder(string patient)
        {
            return new DiagnosticOrder
            {
                // id?
                Id = "143e62fc-eee7-4273-899c-23c60c72cb1a",
                Subject = new ResourceReference { Reference = "Patient/" + patient }, // для примера patient = 106043a2-6600-4590-bedd-6e26c76a6fed
                Orderer = new ResourceReference { Reference = "923cad32-88e6-4ab0-a4cc-5052895b29d9" },
                Encounter = new ResourceReference { Reference = "f0ceca14-6847-4ea4-b128-7c86820da428" },
                SupportingInformation = new List<ResourceReference> { new ResourceReference { Reference = "56350c6f-7333-4002-a622-96968b85381e" } },
                Specimen = new List<ResourceReference> { new ResourceReference { Reference = "f8cd600f-f5b5-4b18-9662-18212c1935f9" } },
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
                                    Url = "urn:oid:1.2.643.2.69.1.100.1",
                                    Value = new CodeableConcept
                                    {
                                         Coding = new List<Coding>
                                         {
                                              new Coding { System = "urn:oid:1.2.643.2.69.1.1.1.32", Code = "1", Version = "1" }
                                         }
                                    }
                                }
                            },
                            Coding = new List<Coding>
                            {
                               new Coding { System = "urn:oid:1.2.643.2.69.1.1.1.31", Code = "B03.016.002", Version = "1"}
                            }
                        }
                    }
                }
            };
        }

        private Specimen SetSpecimen(string patient)
        {
            return new Specimen
            {
                // id?
                Type = new CodeableConcept
                {
                    Coding = new List<Coding> { new Coding { System = "1.2.643.2.69.1.1.1.33", Code = "1", Version = "1" } }
                },
                Subject = new ResourceReference { Reference = "Patient/" + patient },
                Collection = new Specimen.SpecimenCollectionComponent
                {
                    Comment = new List<string> {"Комментарий к биоматериалу"},
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

        private Encounter SetEncounter(string patient)
        {
            return new Encounter
            {
                // id?
                Id = "f0ceca14-6847-4ea4-b128-7c86820da428",
                Identifier = new List<Identifier> { new Identifier{ System = "urn:oid:1.2.643.2.69.1.2.6", Value = "IdCaseMis"}},
                Status = Encounter.EncounterState.InProgress,
                Class = Encounter.EncounterClass.Ambulatory, // class
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
                Indication = new List<ResourceReference> { new ResourceReference { Reference = "71cf33b8-2eae-432d-88d5-747ef8147d0b" } },
                ServiceProvider = new ResourceReference { Reference = "Organization/4bcbf113-f99c-41fa-a92d-43f5684fffc5" }
            };
        }

        private Condition SetCondition(string patient)
        {
            return new Condition
            {
                // id?
                Id = "64d57862-f2c2-41ef-a5cf-27f2d53569eb",
                Identifier = new List<Identifier>()
                {
                    new Identifier
                    {
                    System = "urn:oid:1.2.643.2.69.1.1.1.61",
                    Value = "Стандарт первичной медико-санитарной помощи при хронической болезни почек 4 стадии"
                    }
                },
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

        private Observation SetObservation_BundleOrder()
        {
            return new Observation
            {
                // id??
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

        private Practitioner SetPractitioner()
        {
            return new Practitioner
            {
                Identifier = new List<Identifier>
                {
                    new Identifier
                    {
                        System = "urn:oid:1.2.643.2.69.1.2.6",
                        Value = "IdDoctorMIS" + new Random().Next(100)
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
                        ManagingOrganization = new ResourceReference { Reference = "Organization/4bcbf113-f99c-41fa-a92d-43f5684fffc5" },
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

        //public BundleResult SetBundleResult(string patient)
        //{
        //    return new BundleResult
        //    {
        //        orderResponse = SetOrderResponse(),
        //        diagnosticReport = new DiagnosticReport[] { SetDiagnosticReport() },
        //        observation = new Observation[] { SetObservation_BundleResult() },
        //        practitioner = new Practitioner[] { SetPractitioner() }
        //    };
        //}

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
                Request = new ResourceReference { Reference = "Order/77f3bc81-fd3d-4d8a-8f64-4fe61989f34a" },
                Date = "02.01.2012",
                Who = new ResourceReference { Reference = "Organization/4bcbf113-f99c-41fa-a92d-43f5684fffc5" },
                OrderStatus_ = OrderResponse.OrderStatus.Completed,
                Description = "Комментарий к заказу",
                Fulfillment = new List<ResourceReference>() { new ResourceReference { Reference = "4f6a30fb-cd3c-4ab6-8757-532101f72065" } }
            };
        }

        private DiagnosticReport SetDiagnosticReport()
        {
            return new DiagnosticReport
            {
                Name = new CodeableConcept
                {
                    Coding = new List<Coding>() { new Coding { System = Dictionary.CODE_SERVICE, Code = "B03.016.006", Version = "1" } }
                },
                Status = DiagnosticReport.DiagnosticReportStatus.Final,
                Issued = "03.01.2012",
                Subject = new ResourceReference { Reference = "Patient/106043a2-6600-4590-bedd-6e26c76a6fed" },
                Performer = new ResourceReference { Reference = "3e412c44-1058-40fb-a06f-b9bb9452b39a" },
                RequestDetail = new List<ResourceReference> { new ResourceReference() {Reference = "DiagnosticOrder/2c98670c-3494-4c63-bb29-71acd486da3d" }},
                Result = new List<ResourceReference>() { new ResourceReference { Reference = "651f0cdc-2e7f-4e3a-99b1-da68d2b196c6" } },
                Conclusion = "Текст заключения по услуге B03.016.006",
                //presentedForm ?? 
            };
        }

        private Observation SetObservation_BundleResult()
        {
            return new Observation
            {
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
                Performer = new List<ResourceReference>() { new ResourceReference() {Reference = "3e412c44-1058-40fb-a06f-b9bb9452b39a" }},


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
