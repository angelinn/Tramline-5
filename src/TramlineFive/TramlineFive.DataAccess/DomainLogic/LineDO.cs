﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.Common;
using TramlineFive.DataAccess.Entities;
using TramlineFive.DataAccess.Repositories;

namespace TramlineFive.DataAccess.DomainLogic
{
    public class LineDO
    {
        public LineDO(Line entity)
        {
            id = entity.ID;
            numberString = WebUtility.UrlDecode(entity.Number);
            directions = entity.Directions?.Select(d => new DirectionDO(d));
            type = entity.Type;

            int tempNum;
            if (Int32.TryParse(numberString, out tempNum))
                number = tempNum;
            else
                number = Int32.Parse(numberString[0].ToString());
        }

        public static async Task<IEnumerable<LineDO>> AllAsync()
        {
            return await Task.Run(() =>
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    IEnumerable<Line> lines = uow.Lines.All().ToList();
                    return lines?.Select(l => new LineDO(l));
                }
            });
        }

        public async Task LoadDirections()
        {
            directions = await DirectionDO.GetByLineId(id);
        }

        private int id;

        private VehicleType type;
        public VehicleType Type
        {
            get
            {
                return type;
            }
        }

        private int number;
        public int Number
        {
            get
            {
                return number;
            }
        }

        private string numberString;
        public string NumberString
        {
            get
            {
                return numberString;
            }
        }

        private IEnumerable<DirectionDO> directions;
        public IEnumerable<DirectionDO> Directions
        {
            get
            {
                return directions;
            }
        }

        public string TypeToString()
        {
            switch (type)
            {
                case VehicleType.Bus:
                    return "Автобус";
                case VehicleType.Tram:
                    return "Трамвай";
                case VehicleType.Trolley:
                    return "Тролей";
                default:
                    return String.Empty;
            }
        }

        public override string ToString()
        {
            string stringType = TypeToString();

            return $"{stringType} {numberString}";
        }
    }
}