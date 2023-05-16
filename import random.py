import random

BOSS_HEALTH = 100
PLAYER_HEALTH = 10
BOSS_DAMAGE = 20
PLAYER_DAMAGE = 2
NUM_GENERATIONS = 1000
POPULATION_SIZE = 100
MUTATION_RATE = 0.1

def generate_random_moves():
    return [random.randint(0, 2) for _ in range(10)]

def calculate_damage(attacker_moves, defender_moves, attacker_damage, defender_health):
    for attacker_move, defender_move in zip(attacker_moves, defender_moves):
        if attacker_move == 0 and defender_move == 1:
            defender_health -= attacker_damage
        elif attacker_move == 1 and defender_move == 2:
            defender_health -= attacker_damage
        elif attacker_move == 2 and defender_move == 0:
            defender_health -= attacker_damage
    return defender_health

def fitness(moves):
    boss_health = BOSS_HEALTH
    player_health = PLAYER_HEALTH
    for i in range(5):
        boss_health = calculate_damage(moves[i * 2:(i + 1) * 2], moves[(i + 1) * 2:(i + 2) * 2], PLAYER_DAMAGE, boss_health)
        if boss_health <= 0:
            return i + 1
        player_health = calculate_damage(moves[(i + 1) * 2:(i + 2) * 2], moves[i * 2:(i + 1) * 2], BOSS_DAMAGE, player_health)
        if player_health <= 0:
            return 0
    return 6

def crossover(moves1, moves2):
    crossover_point = random.randint(1, len(moves1) - 1)
    child_moves = moves1[:crossover_point] + moves2[crossover_point:]
    return child_moves

def mutate(moves):
    for i in range(len(moves)):
        if random.random() < MUTATION_RATE:
            moves[i] = random.randint(0, 2)
    return moves

def generate_initial_population():
    population = []
    for i in range(POPULATION_SIZE):
        moves = generate_random_moves()
        population.append(moves)
    return population

def run_genetic_algorithm():
    population = generate_initial_population()

    for generation in range(NUM_GENERATIONS):
        population = sorted(population, key=fitness, reverse=True)
        if fitness(population[0]) == 6:
            print(f"Boss Health: {BOSS_HEALTH} | Player Health: {PLAYER_HEALTH}")
            return population[0]
        print(f"Generation {generation+1}: Boss health - {BOSS_HEALTH}, Player health - {PLAYER_HEALTH}")
        print(f"Player moves: {population[0]}")
        new_population = [population[0]]
        while len(new_population) < POPULATION_SIZE:
            parent1 = random.choice(population)
            parent2 = random.choice(population)
            child = crossover(parent1, parent2)
            child = mutate(child)
            new_population.append(child)
        population = new_population
        BOSS_HEALTH = calculate_damage(population[0][:2], population[0][2:4], PLAYER_DAMAGE, BOSS_HEALTH)
        PLAYER_HEALTH = calculate_damage(population[0][2:4], population[0][:2], BOSS_DAMAGE, PLAYER_HEALTH)
    print(f"Boss Health: {BOSS_HEALTH} | Player Health: {PLAYER_HEALTH}")

    return population[0]

result = run_genetic_algorithm()
print(f"Player evolved and defeated the boss using moves: {result}")
