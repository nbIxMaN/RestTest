using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                address = new Address[]
                {
                    new Address() { Use = "home", text = RandomAddress() }
                },
                identifier = new Identifier[]
                {
                    new Identifier()
                    {
                        system = "urn:oid:1.2.643.2.69.1.2.6",
                        value = "IdPatientMis" + new Random().Next(1000),
                        assigner = new Reference() { reference = "Link/4bcbf113-f99c-41fa-a92d-43f5684fffc5" },
                        //period  = new Period(Convert.ToDateTime("01.02.2012"), Convert.ToDateTime("01.02.2018")) для паспорта
                    }
                },
                birthDate = Convert.ToDateTime(RandomBirthDate()),
                gender = "male",
                name = new HumanName()
                {
                    family = new string[] { RandomFIO()[0] },
                    given = new string[] { RandomFIO()[1], RandomFIO()[2] }
                }
            };
        }

        public Coverage SetCoverage(string patient)
        {
            return new Coverage
            {
                type = new Coding { system = Dictionary.TYPE_COVERAGE, code = "2", version = 1 },
                subscriber = new Reference { reference = "Patient/" + patient }, // для примера patient = 106043a2-6600-4590-bedd-6e26c76a6fed
                identifier = new Identifier
                {
                    system = "urn:oid:1.2.643.5.1.13.2.1.1.635.23607",//system = ...635.[код страховой компании]
                    value = "1234567891011121",
                    //period = new Period(Convert.ToDateTime("01.02.2012"), Convert.ToDateTime("01.02.2018"))
                }
            };
        }

        public BundleOrder SetBundleOrder(string patient)
        {
            return new BundleOrder
            {
                order = SetOrder(patient),
                diagnosticOrder = new DiagnosticOrder[] { SetDiagnosticOrder(patient) },
                specimen = new Specimen[] { SetSpecimen(patient) },
                encounter = SetEncounter(patient),
                condition = new Condidtion[] { SetCondition(patient) },
                observation = new Observation[] { SetObservation() },
                practitioner = new Practitioner[] { SetPractitioner() },
            };
        }
        private Order SetOrder(string patient)
        {
            return new Order
            {
                identifier = new Identifier
                {
                    system = "urn:oid:1.2.643.2.69.1.2.6",
                    value = "IdOrderMis" + new Random().Next(1000)
                },
                date = Convert.ToDateTime("01.01.2012"),
                subject = new Reference { reference = "Patient/" + patient }, // для примера patient = 106043a2-6600-4590-bedd-6e26c76a6fed
                source = new Reference { reference = "519a08f4-c240-4e58-aa66-fe2a017b8d94" },
                target = new Reference { reference = "Organization/4bcbf113-f99c-41fa-a92d-43f5684fffc5" },
                detail = new Reference[] { new Reference { reference = "143e62fc-eee7-4273-899c-23c60c72cb1a" } },
                when = new When
                {
                    code = new CodeableConcept
                    {
                        coding = new Coding[]
                        {
                            new Coding { system = Dictionary.PRIORITY_EXECUTION, code = "Routine", version = 1 }
                        }
                    }
                }
            };
        }

        private DiagnosticOrder SetDiagnosticOrder(string patient)
        {
            return new DiagnosticOrder
            {
                subject = new Reference { reference = "Patient/" + patient }, // для примера patient = 106043a2-6600-4590-bedd-6e26c76a6fed
                orderer = new Reference { reference = "923cad32-88e6-4ab0-a4cc-5052895b29d9" },
                encounter = new Reference { reference = "f0ceca14-6847-4ea4-b128-7c86820da428" },
                supportingInformation = new Reference[] { new Reference { reference = "56350c6f-7333-4002-a622-96968b85381e" } },
                specimen = new Reference[] { new Reference { reference = "f8cd600f-f5b5-4b18-9662-18212c1935f9" } },
                status = "requested",
                item = new Item[]
                {
                    new Item
                    {
                        //??
                    }
                }
            };
        }

        private Specimen SetSpecimen(string patient)
        {
            return new Specimen
            {
                type = new CodeableConcept
                {
                    coding = new Coding[] { new Coding { system = "1.2.643.2.69.1.1.1.33", code = "1", version = 1 } }
                },
                subject = new Reference { reference = "Patient/" + patient },
                // collection ??
                container = new Container
                {
                    identifier = new Identifier { system = "http://netrika.ru/container-type-identifier", value = "barcode111" }, // system?
                    type = new CodeableConcept
                    {
                        coding = new Coding[] { new Coding { system = Dictionary.TYPE_CONTAINER, code = "1", version = 1 } }
                    }
                }

            };
        }

        private Encounter SetEncounter(string patient)
        {
            return new Encounter
            {
                identifier = new Identifier { system = "urn:oid:1.2.643.2.69.1.2.6", value = "IdCaseMis" + new Random().Next(1000) },
                status = "in-progress",
                clas = "ambulatory",
                type = new CodeableConcept
                {
                    coding = new Coding[] { new Coding { system = Dictionary.TYPE_CASE, code = "2", version = 1 } }
                },
                patient = new Reference { reference = "Patient/" + patient },
                reason = new CodeableConcept
                {
                    coding = new Coding[] { new Coding { system = Dictionary.REASON, code = "1", version = 1 } }
                },
                indication = new Reference[] { new Reference { reference = "71cf33b8-2eae-432d-88d5-747ef8147d0b" } },
                serviceProvider = new Reference { reference = "Organization/4bcbf113-f99c-41fa-a92d-43f5684fffc5" }
            };
        }

        private Condidtion SetCondition(string patient)
        {
            return new Condidtion
            {
                identifier = new Identifier
                {
                    system = "1.2.643.2.69.1.1.1.61",
                    value = "Стандарт первичной медико-санитарной помощи при хронической болезни почек 4 стадии"
                },
                subject = new Reference { reference = "Patient/" + patient },
                dateAsserted = Convert.ToDateTime("01.02.2012"),
                code =  new CodeableConcept
                {
                    coding = new Coding[] { new Coding { system = Dictionary.DIAGNOSIS, code = "N18.9", version = 1 } }
                },
                category = new CodeableConcept
                {
                    coding = new Coding[] { new Coding { system = Dictionary.TYPE_CONDITION, code = "diagnosis", version = 1 } }
                },
                clinicalStatus = "confirmed",
                notes = "Уточнение",
                //dueTo.target?
            };
        }

        private Practitioner SetPractitioner()
        {
            return new Practitioner
            {

            };
        }

        private Observation SetObservation()
        {
            throw new NotImplementedException();
        }
    }
}
