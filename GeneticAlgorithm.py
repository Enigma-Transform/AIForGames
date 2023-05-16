import random

TARGET_STRING = "hello world this is an electirc sheep"
POPULATION_SIZE = 200
MUTATION_RATE = 0.01
TOURNAMENT_SIZE = 3
ELITISM = True
MAX_GENERATIONS = 10000

def generate_random_string(length):
    return "".join(random.choices("abcdefghijklmnopqrstuvwxyz ", k=length))

def fitness(string):
    return sum(1 for a, b in zip(string, TARGET_STRING) if a == b)

def select_individuals(population, tournament_size):
    tournament = random.sample(population, tournament_size)
    return max(tournament, key=fitness)

def crossover(parent1, parent2):
    point1 = random.randint(0, len(parent1) - 1)
    point2 = random.randint(point1, len(parent1))
    child = parent1[:point1] + parent2[point1:point2] + parent1[point2:]
    return child

def mutate(string, mutation_rate):
    return "".join(c if random.random() > mutation_rate else random.choice("abcdefghijklmnopqrstuvwxyz ") for c in string)

def create_initial_population(population_size):
    return [generate_random_string(len(TARGET_STRING)) for _ in range(population_size)]

def run_genetic_algorithm():
    population = create_initial_population(POPULATION_SIZE)
    for generation in range(MAX_GENERATIONS):
        population = sorted(population, key=fitness, reverse=True)
        print(f"Generation {generation}: {population[0]}")
        if fitness(population[0]) == len(TARGET_STRING):
            print(f"Found target string '{population[0]}' after {generation} generations")
            return population[0]
        
        next_population = []
        if ELITISM:
            next_population.append(population[0])
            
        for i in range(len(population) - len(next_population)):
            parent1 = select_individuals(population, TOURNAMENT_SIZE)
            parent2 = select_individuals(population, TOURNAMENT_SIZE)
            child = crossover(parent1, parent2)
            mutated_child = mutate(child, MUTATION_RATE)
            next_population.append(mutated_child)
        
        population = next_population
    
    print(f"Could not find target string '{TARGET_STRING}' after {MAX_GENERATIONS} generations")
    return None

result = run_genetic_algorithm()
if result:
    print(result)
