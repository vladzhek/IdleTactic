using System;
using System.Collections.Generic;
using Slime.Configs.Abstract;
using Slime.Data.Summon;
using UnityEngine;

namespace Slime.Configs.Summon
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Slime.Data.Constants.System.ASSET_PATH + ENTITY, order = 0)]
    public class SummonConfig : ItemsConfig<SummonItemConfig, SummonBaseData>
    {
        private const string ENTITY = "Summon";

        // public
        public override IEnumerable<SummonBaseData> Items => throw new NotImplementedException();
        public SummonData Data => _data;
        public SummonAdData AdData => _adData;

        // private
        
        // ReSharper disable once InconsistentNaming
        //[HideInInspector] protected new SummonData[] _items;
        //[Header("Summon")]
        [SerializeField] private SummonData _data;
        [Space]
        [SerializeField] private SummonAdData _adData;
    }

    public class SummonItemConfig : ItemConfig<SummonBaseData>
    {
        
    }
}