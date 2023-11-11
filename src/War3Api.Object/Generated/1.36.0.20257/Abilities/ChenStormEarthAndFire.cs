using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using War3Api.Object.Abilities;
using War3Api.Object.Enums;
using War3Net.Build.Object;
using War3Net.Common.Extensions;

namespace War3Api.Object.Abilities
{
    public sealed class ChenStormEarthAndFire : Ability
    {
        public ChenStormEarthAndFire(): base(1717920577)
        {
        }

        public ChenStormEarthAndFire(int newId): base(1717920577, newId)
        {
        }

        public ChenStormEarthAndFire(string newRawcode): base(1717920577, newRawcode)
        {
        }

        public ChenStormEarthAndFire(ObjectDatabaseBase db): base(1717920577, db)
        {
        }

        public ChenStormEarthAndFire(int newId, ObjectDatabaseBase db): base(1717920577, newId, db)
        {
        }

        public ChenStormEarthAndFire(string newRawcode, ObjectDatabaseBase db): base(1717920577, newRawcode, db)
        {
        }
    }
}