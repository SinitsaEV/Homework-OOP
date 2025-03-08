using System;
using System.Collections.Generic;
using System.Text;

namespace OOP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            BattlefieldCreator creator = new BattlefieldCreator();
            Battlefield battlefield = creator.Create();
            battlefield.InitialiseWar();
        }
    }

    public interface IDamageable
    {
        void TakeDamage(int damage);
    }

    class RegularSoldier : IDamageable
    {
        public RegularSoldier(string name)
        {
            Name = name;
            Health = 100;
            Armor = 10;
            Damage = 20;
        }

        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public int Health { get; protected set; }
        public int Armor { get; protected set; }
        public int Damage { get; protected set; }

        public void TakeDamage(int damage)
        {
            int currentDamage = damage - Armor;

            if (currentDamage > 0)
            {                
                Health -= currentDamage;
                Console.WriteLine($"{Name} получил {currentDamage} урона. Осталось: {Health}");
            }
        }

        public virtual RegularSoldier Clone()
        {
            return new RegularSoldier(Name);
        }

        public virtual void Attack(List<IDamageable> damageables)
        {
            IDamageable target = GetRandomTarget(damageables);

            if (target != null)
            {
                Console.WriteLine($"{Name} атакует:");
                target.TakeDamage(Damage);
            }
        }

        protected IDamageable GetRandomTarget(List<IDamageable> targets)
        {
            int randomIndex = UserUtils.GenerateRandomNumber(0, targets.Count);

            return targets[randomIndex];
        }
    }

    class SniperSoldier : RegularSoldier
    {
        private int _damageMultiplier;

        public SniperSoldier(string name, int damageMultiplier) : base(name)
        {
            _damageMultiplier = damageMultiplier;
        }

        public override void Attack(List<IDamageable> damageables)
        {
            IDamageable target = GetRandomTarget(damageables);

            if (target != null)
            {
                Console.WriteLine($"{Name} атакует:");
                target.TakeDamage(Damage * _damageMultiplier);
            }
        }
    }

    class MultiTargetSoldier : RegularSoldier
    {
        private int _targetsCount;

        public MultiTargetSoldier(string name, int targetsCount) : base(name)
        {
            _targetsCount = targetsCount;
        }

        public List<IDamageable> GetRandomTargets(List<IDamageable> damageables)
        {
            List<IDamageable> targets = new List<IDamageable>();
            IDamageable target;

            if (_targetsCount >= damageables.Count)
            {
                return damageables;
            }

            for(int i = 0; i < _targetsCount; i++)
            {
                target = GetRandomTarget(damageables);

                if (targets.Contains(target))
                {
                    continue;
                }

                targets.Add(target);
            }

            return targets;
        }

        public override void Attack(List<IDamageable> damageables)
        {
            List<IDamageable> targets = GetRandomTargets(damageables);

            if (damageables != null)
            {
                Console.WriteLine($"{Name} атакует:");

                foreach (IDamageable target in targets)
                {
                    target.TakeDamage(Damage);
                }
            }
        }

        public override RegularSoldier Clone()
        {
            return new MultiTargetSoldier(Name, _targetsCount);
        }
    }

    class ScatterSoldier : RegularSoldier
    {
        private int _targetsCount;

        public ScatterSoldier(string name, int targetsCount) : base(name)
        {
            _targetsCount = targetsCount;
        }

        public List<IDamageable> GetRandomTargets(List<IDamageable> damageables)
        {
            List<IDamageable> targets = new List<IDamageable>();

            if (_targetsCount >= damageables.Count)
            {
                return damageables;
            }

            for (int i = 0; i < _targetsCount; i++)
            {
                targets.Add(GetRandomTarget(damageables));
            }

            return targets;
        }

        public override void Attack(List<IDamageable> damageables)
        {
            List<IDamageable> targets = GetRandomTargets(damageables);

            if (damageables != null)
            {
                Console.WriteLine($"{Name} атакует:");

                foreach (IDamageable target in targets)
                {
                    target.TakeDamage(Damage);
                }
            }
        }

        public override RegularSoldier Clone()
        {
            return new ScatterSoldier(Name, _targetsCount);
        }
    }

    class Platoon
    {
        private List<RegularSoldier> _soldiers;

        public Platoon(List<RegularSoldier> soldiers, string name)
        {
            _soldiers = soldiers;
            Name = name;
        }

        public string Name { get; private set; }

        public void Attack(List<IDamageable> damageables)
        {
            Console.WriteLine($"{Name} атакует:");

            foreach (RegularSoldier soldier in _soldiers)
            {
                soldier.Attack(damageables);
            }
        }

        public Platoon Clone()
        {
            List<RegularSoldier> soldiers = new List<RegularSoldier>();

            foreach(RegularSoldier soldier in _soldiers)
            {
                soldiers.Add(soldier.Clone());
            }

            return new Platoon(soldiers, Name);
        }

        public List<IDamageable> GetDamageables()
        {
            return new List<IDamageable>(_soldiers);
        }

        public void RemoveDeadSoldiers()
        {
            foreach(RegularSoldier soldier in _soldiers)
            {
                if (soldier.Health <= 0)
                {
                    Console.WriteLine($"{soldier.Name} пал.");
                    _soldiers.Remove(soldier);
                }
            }
        }
    }

    class Battlefield
    {
        private Platoon _firstPlatoon;
        private Platoon _secondPlatoon;

        public Battlefield(string name, Platoon firstPlatoon, Platoon secondPlatoon)
        {
            Name = name;
            _firstPlatoon = firstPlatoon;
            _secondPlatoon = secondPlatoon;
        }

        public string Name { get; private set; }

        public void InitialiseWar()
        {
            const string ShowBattleCommand = "1";
            const string ExitCommand = "2";

            Console.WriteLine($"Добро пожаловать на поле боя {Name}");

            bool isActive = true;

            while (isActive)
            {
                Console.WriteLine($"{ShowBattleCommand} - посмотреть бой\n{ExitCommand} - выйти");
                Console.Write("Введите команду: ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case ShowBattleCommand:
                        SimulateBattle();
                        break;
                    case ExitCommand:
                        isActive = false;
                        Console.WriteLine("Вы вышли.");
                        break;
                    default:
                        Console.WriteLine("Неверный ввод.");
                        break;
                }
            }
        }

        private void SimulateBattle()
        {
            Platoon firstPlatoon = _firstPlatoon.Clone();
            Platoon secondPlatoon = _secondPlatoon.Clone();

            while (firstPlatoon.GetDamageables().Count > 0 && secondPlatoon.GetDamageables().Count > 0) 
            {
                SimulateRound(firstPlatoon, secondPlatoon);
            }

            DetermineWinner(firstPlatoon, secondPlatoon);
        }               

        private void SimulateRound(Platoon firstPlatoon, Platoon secondPlatoon)
        {
            firstPlatoon.Attack(secondPlatoon.GetDamageables());
            secondPlatoon.RemoveDeadSoldiers();

            secondPlatoon.Attack(firstPlatoon.GetDamageables());
            firstPlatoon.RemoveDeadSoldiers();
        }

        private void DetermineWinner(Platoon firstPlatoon, Platoon secondPlatoon)
        {
            if (firstPlatoon.GetDamageables().Count > 0)
            {
                Console.WriteLine($"{firstPlatoon.Name} победил.");
            }
            else if(secondPlatoon.GetDamageables().Count > 0)
            {
                Console.WriteLine($"{secondPlatoon.Name} победил.");
            }
        }
    }

    class BattlefieldCreator
    {
        public Battlefield Create()
        {
            return new Battlefield("Америка", new Platoon(CreateSoldiers(), "Взвод 1"), new Platoon(CreateSoldiers(), "Взвод 2"));
        }

        private List<RegularSoldier> CreateSoldiers()
        {
            return new List<RegularSoldier> {

                new RegularSoldier("Обычный солдат 1"),
                new RegularSoldier("Обычный солдат 2"),
                new SniperSoldier("Солдат с множителем урона 1",3),
                new SniperSoldier("Солдат с множителем урона 2",3),
                new MultiTargetSoldier("Солдат атакующий 3-целей без повторения 1",3),
                new MultiTargetSoldier("Солдат атакующий 4-целей без повторения 2",4),
                new ScatterSoldier("Солдат атакующий 3-целей c повторениями 1",3),
                new ScatterSoldier("Солдат атакующий 4-целей c повторениями 2",4)
            };
        }
    }

    class UserUtils
    {
        private static Random s_random = new Random();

        public static int GenerateRandomNumber(int min, int max)
        {
            return s_random.Next(min, max);
        }
    }
}