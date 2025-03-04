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

            ArenaCreator creator = new ArenaCreator();
            Arena arena = creator.Create();
            arena.ShowMenu();
        }
    }

    class ArenaCreator
    {
        public Arena Create()
        {
            List<Fighter> fighters = new List<Fighter>();

            fighters.Add(new FireMage("Mage",100,20,15));
            fighters.Add(new Doubler("Doubler", 15));
            fighters.Add(new DoubleStrike("DoubleStrike", 3));
            fighters.Add(new FuriousDefender("FuriousDefender"));
            fighters.Add(new EvasiveWarrior("EvasiveWarrior", 15));

            return new Arena(fighters);
        }
    }

    static class FighterCreator
    {
        static public Fighter Create(Fighter fighter)
        {
            switch (fighter)
            {
                case FireMage fireMage:
                    return new FireMage(fireMage.Name, fireMage.Mana, fireMage.FireBallCost, fireMage.FireBallDamage);

                case Doubler doubler:
                    return new Doubler(doubler.Name, doubler.DoubleDamageChance);

                case DoubleStrike doubleStrike:
                    return new DoubleStrike(doubleStrike.Name, doubleStrike.BonusAttackFrequency);

                case EvasiveWarrior evasiveWarrior:
                    return new EvasiveWarrior(evasiveWarrior.Name, evasiveWarrior.DodgeChance);

                case FuriousDefender defender:
                    return new FuriousDefender(defender.Name);

                default:
                    return null;
            }
        }
    }

    class Arena
    {
        private List<Fighter> _fighterList;

        public Arena(List<Fighter> fighterList)
        {
            _fighterList = fighterList;
        }

        public void ShowMenu()
        {            
            const string ShowWelcomeMessageCommand = "1";
            const string ShowBattleCommand = "2";
            const string ExitCommand = "3";
            const string WelcomeMessage = "Hello";
            
            bool isActive = true;

            while (isActive)
            {
                Console.WriteLine($"{ShowWelcomeMessageCommand} - посмотреть стартовое сообщение" +
                    $"\n{ShowBattleCommand} - посмотреть бой" +
                    $"\n{ExitCommand} - выйти");

                string playerInput = Console.ReadLine();

                switch (playerInput)
                {
                    case ShowWelcomeMessageCommand:
                        Console.WriteLine(WelcomeMessage);
                        break;

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
            List<Fighter> fighters = new List<Fighter>();
            fighters.Add(ChooseFighter());
            fighters.Add(ChooseFighter());

            while (fighters[0].CurrentHealth > 0 && fighters[1].CurrentHealth > 0)
            {
                int randomIndex = UserUtils.GenerateRandomNumber(0, fighters.Count - 1);

                HitTarget(fighters[randomIndex], fighters[fighters.Count - 1 - randomIndex]);
                HitTarget(fighters[fighters.Count - 1 - randomIndex], fighters[randomIndex]);

                foreach(Fighter fighter in fighters)
                {
                    Console.WriteLine($"{fighter.Name} - {fighter.CurrentHealth} xp");
                }
            }

            DetermineWinner(fighters);
        }

        private void DetermineWinner(List<Fighter> fighters)
        {
            if (fighters[0].CurrentHealth > 0)
            {
                Console.WriteLine(fighters[0].Name + " победил");
            }
            else if (fighters[1].CurrentHealth > 0)
            {
                Console.WriteLine(fighters[1].Name + " победил");
            }
            else
            {
                Console.WriteLine("ничья");
            }
        }

        private void HitTarget(Fighter attacker, Fighter target)
        {
            switch (attacker)
            {
                case FireMage fireMage:
                    fireMage.Attack(target);
                    break;

                case Doubler doubler:
                    doubler.Attack(target);
                    break;

                case DoubleStrike doubleStrike:
                    doubleStrike.Attack(target);
                    break;

                case EvasiveWarrior evasiveWarrior:
                    evasiveWarrior.Attack(target);
                    break;

                case FuriousDefender defender:
                    defender.Attack(target);
                    break;

                default:
                    Console.WriteLine();
                    break;
            }
        }

        private Fighter ChooseFighter()
        {
            ShowFighters();

            bool isCorrectInput = false;

            Console.Write("Выберите бойца.");

            while (isCorrectInput == false)
            {
                if(int.TryParse(Console.ReadLine(), out int fighterIndex))
                {
                    if (fighterIndex >= 0 && fighterIndex < _fighterList.Count)
                    {
                        isCorrectInput = true;

                        return FighterCreator.Create(_fighterList[fighterIndex]);
                    }
                    else
                    {
                        Console.WriteLine("Неверный ввод попробуй еще раз.");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод попробуй еще раз.");
                }
            }

            return null;
        }

        private void ShowFighters()
        {
            foreach (Fighter fighterInList in _fighterList)
            {
                Console.WriteLine(_fighterList.FindIndex(fighter => fighter.Name == fighterInList.Name) + " " + fighterInList.Name);
            }
        }
    }    

    public interface IDamageable
    {
        void TakeDamage(int damage);
    }

    abstract class Fighter : IDamageable
    {
        protected Fighter(string name)
        {
            Name = name;
            Damage = 10;
            MaxHealth = 150;
            CurrentHealth = MaxHealth;
            Armor = 5;
        }
        public int Damage { get; protected set; }
        public int MaxHealth { get; protected set; }
        public int CurrentHealth { get; protected set; }
        public int Armor { get; protected set; }
        public string Name { get; protected set; }

        public virtual void TakeDamage(int damage)
        {
            if (damage > 0)
            {
                CurrentHealth -= damage - Armor;
            }
        }

        public virtual void Attack(IDamageable damageable)
        {
            Console.WriteLine($"{Name} нанес {Damage} урона");
            damageable.TakeDamage(Damage);
        }
    }

    class Doubler : Fighter
    {
        public Doubler(string name, int doubleDamageChance) : base(name)
        {
            DoubleDamageChance = doubleDamageChance;
            DoubleDamageMultiplier = 2;
        }

        public int DoubleDamageChance { get; private set; }
        public int DoubleDamageMultiplier { get; private set; }

        public override void Attack(IDamageable damageable)
        {
            if (TryDoubleDamage(damageable) == false)
            {
                base.Attack(damageable);
            }
        }

        private bool TryDoubleDamage(IDamageable damageable)
        {
            int max = 100;
            int min = 0;

            int randomIndex = UserUtils.GenerateRandomNumber(min, max);

            if(randomIndex <= DoubleDamageChance)
            {
                damageable.TakeDamage(Damage * DoubleDamageMultiplier);
                Console.WriteLine($"{Name} нанес {Damage * DoubleDamageMultiplier} урона");
                return true;
            }

            return false;
        }
    }

    class DoubleStrike : Fighter
    {
        public DoubleStrike(string name, int bonusAttackFrequency) : base(name)
        {
            BonusAttackFrequency = bonusAttackFrequency;
            ComboProgress = 0;
        }

        public int BonusAttackFrequency { get; private set; }
        public int ComboProgress { get; private set; }

        public override void Attack(IDamageable damageable)
        {
            TryBonusAttack(damageable);

            base.Attack(damageable);
        }

        private void TryBonusAttack(IDamageable damageable)
        {
            if (ComboProgress == BonusAttackFrequency)
            {
                ComboProgress = 0;
                Console.WriteLine($"{Name} нанес {Damage} урона бонусной атакой");
                damageable.TakeDamage(Damage);
            }
            else
            {
                ComboProgress++;
            }
        }        
    }

    class FuriousDefender : Fighter
    {
        public FuriousDefender(string name) : base(name) 
        {
            MaxRageValue = 100;
            CurrentRageValue = 0;
            RageHealValue = 10;
        }

        public int MaxRageValue { get; private set; }
        public int CurrentRageValue { get; private set; }
        public int RageHealValue { get; private set; }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            if((CurrentRageValue += damage) > MaxRageValue)
            {
                CurrentRageValue = MaxRageValue;
            }
        }

        public override void Attack(IDamageable damageable)
        {
            if(TryRageHeal() == false)
            {
                base.Attack(damageable);
            }
        }

        private bool TryRageHeal()
        {
            if(CurrentRageValue == MaxRageValue)
            {
                CurrentHealth += RageHealValue;
                CurrentRageValue = 0;

                if(CurrentHealth > MaxHealth)
                {
                    CurrentHealth = MaxHealth;
                }

                Console.WriteLine($"{Name} восстановил немнго здоровья");
                return true;
            }

            return false;
        }
    }

    class FireMage : Fighter
    {
        public FireMage(string name, int mana, int fireBallCost, int fireBallDamage) : base(name)
        {
            Mana = mana;
            FireBallCost = fireBallCost;
            FireBallDamage = fireBallDamage;
        }

        public int Mana { get; private set; }
        public int FireBallCost { get; private set; }
        public int FireBallDamage { get; private set; }

        public override void Attack(IDamageable damageable)
        {
            if (TryCastFireBall(damageable) == false)
            {
                base.Attack(damageable);
            }
        }

        private bool TryCastFireBall(IDamageable fighter)
        {
            if(Mana >= FireBallCost)
            {
                Mana -= FireBallCost;
                fighter.TakeDamage(FireBallDamage);
                Console.WriteLine($"{Name} нанес {Damage} урона фаерболом");
                return true;
            }

            return false;
        }
    }

    class EvasiveWarrior : Fighter
    {
        public EvasiveWarrior(string name, int dodgeChance) : base(name) 
        {
            DodgeChance = dodgeChance;
        }

        public int DodgeChance { get; private set; }

        public override void TakeDamage(int damage)
        {
            if (TryDodgeAttack() == false) 
            {
                base.TakeDamage(damage);
            }
            else
            {
                Console.WriteLine(Name + "уклонился");
            }
        }

        private bool TryDodgeAttack()
        {
            int max = 100;
            int min = 0;

            int randomIndex = UserUtils.GenerateRandomNumber(min, max);

            if(randomIndex < DodgeChance)
            {                
                return true;
            }

            return false;
        }
    }

    class UserUtils
    {
        private static Random random = new Random();

        public static int GenerateRandomNumber(int min, int max)
        {
            return random.Next(min, max + 1);
        }

        public static int GenerateRandomNumber(int max)
        {
            return random.Next(max + 1);
        }
    }
}