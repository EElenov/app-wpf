using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace app_domain
{
    /// <summary>
    /// This is just data fill controller for presentation purpouses
    /// will serve as a database management class
    /// </summary>
    public static class BogusFill
    {
        public static readonly Random RND = new Random();
        private static readonly Faker faker = new Faker();

        public static List<DummyDesign> FillDataForDesigns(int genlength)
        {
            var root = new Faker<DummyDesign>("en")
                .RuleFor(x => x.Id, x => x.Random.Hash())
                .RuleFor(x => x.Name, x => x.Name.JobArea())
                .RuleFor(x => x.Description, x => x.Lorem.Sentences(sentenceCount: RND.Next(1, 5)))
                .RuleFor(x => x.Tooltip, x => x.Lorem.Sentences(sentenceCount: RND.Next(1, 2)))
                .RuleFor(x => x.CreationDate, x => x.Date.Past())
                .Ignore(x => x.Programs);
            return root.Generate(genlength);
        }

        public static List<DummyProgram> FillDataForPrograms(int genlength, string hash)
        {
            var root = new Faker<DummyProgram>("en")
                .Ignore(x => x.Id)
                .RuleFor(x => x.Name, x => x.Name.JobTitle())
                .RuleFor(x => x.Description, x => x.Lorem.Sentences(sentenceCount: RND.Next(1, 7)))
                .RuleFor(x => x.Tooltip, x => x.Lorem.Sentences(sentenceCount: RND.Next(1, 2)))
                .RuleFor(x => x.CreationDate, x => x.Date.Past())
                .Ignore(x => x.Parameters);
            var retval = root.Generate(genlength);
            retval.ForEach(x => x.Id = hash);
            return retval;
        }

        public static List<DummyParameter> FillDataForParameters(int genlength)
        {
            var root = new Faker<DummyParameter>("en")
                .RuleFor(x => x.Name, x => x.Database.Column())
                .RuleFor(x => x.Tooltip, x => x.Lorem.Sentences(sentenceCount: RND.Next(1, 10)))
                .RuleFor(x => x.ParamInterpreter, x => x.PickRandom<ParameterType>())
                .Ignore(x => x.Data);
            var retval = root.Generate(genlength);
            for (int i = 0; i < retval.Count; i++)
            {
                switch (retval[i].ParamInterpreter)
                {
                    case ParameterType.IsInteger:
                        retval[i].Data = faker.Random.Int();
                        break;
                    case ParameterType.IsString:
                        retval[i].Data = faker.Random.Words(RND.Next(1, 3));
                        break;
                    case ParameterType.IsBoolean:
                        retval[i].Data = faker.Random.Bool();
                        break;
                    case ParameterType.IsCollection:
                        if (RND.Next(1, 10) < 8)
                        {
                            retval[i].Data = faker.Make(RND.Next(2, 10), () => faker.Random.Word());
                            continue;
                        }
                        retval[i].Data = faker.Make(RND.Next(2, 8), () => faker.Random.Number());
                        break;
                    default: break;
                }
            }
            return retval;
        }
    }
}
