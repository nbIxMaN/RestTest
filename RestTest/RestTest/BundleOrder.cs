﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestTest
{
    //убрала массивы
    class BundleOrder: Resource
    {
        public Order order;
        public Patient patient;
        public Practitioner practitioner;
        public DiagnosticOrder diagnosticOrder;
        public Specimen specimen;
        public Encounter encounter;
        public Condition condition;
        public Observation observation;
        public Coverage coverage;
    }
}
