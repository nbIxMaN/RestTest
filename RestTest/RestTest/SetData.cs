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
                diagnosticOrder = new DiagnosticOrder[] { SetDiagnosticOrder() },
                specimen = new Specimen[] { SetSpecimen() },
                encounter = SetEncounter(),
                condition = new Condidtion[] { SetCondition() },
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

        private DiagnosticOrder SetDiagnosticOrder()
        {
            return new DiagnosticOrder
            {

            };
        }

        private Practitioner SetPractitioner()
        {
            throw new NotImplementedException();
        }

        private Observation SetObservation()
        {
            throw new NotImplementedException();
        }

        private Condidtion SetCondition()
        {
            throw new NotImplementedException();
        }

        private Encounter SetEncounter()
        {
            throw new NotImplementedException();
        }

        private Specimen SetSpecimen()
        {
            throw new NotImplementedException();
        }

       


    }
}
