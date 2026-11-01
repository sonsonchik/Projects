# используется для сортировки
from operator import itemgetter

class Driver:
    """Водитель"""
    def __init__(self, id, fio, exp, park_id):
        self.id = id
        self.fio = fio
        self.exp = exp       # количественный признак (стаж)
        self.park_id = park_id

class Park:
    """Автопарк"""
    def __init__(self, id, name):
        self.id = id
        self.name = name

class DriverPark:
    """
    'Водители автопарка' для реализации связи многие-ко-многим
    """
    def __init__(self, park_id, driver_id):
        self.park_id = park_id
        self.driver_id = driver_id

# Автопарки
parks = [
    Park(1, 'Центральный автопарк'),
    Park(2, 'Северный автопарк'),
    Park(3, 'Южный автопарк'),
]

# Водители
drivers = [
    Driver(1, 'Иванов', 10, 1),
    Driver(2, 'Петров', 5, 2),
    Driver(3, 'Сидоров', 8, 3),
    Driver(4, 'Смирнов', 3, 3),
    Driver(5, 'Кузнецов', 12, 1),
]

# Связь многие-ко-многим
drivers_parks = [
    DriverPark(1, 1),
    DriverPark(2, 2),
    DriverPark(3, 3),
    DriverPark(3, 4),
    DriverPark(1, 5),
]

def main():
    """Основная функция"""
    # Соединение данных один-ко-многим
    one_to_many = [(d.fio, d.exp, p.name)
                   for p in parks
                   for d in drivers
                   if d.park_id == p.id]

    # Соединение данных многие-ко-многим
    many_to_many_temp = [(p.name, dp.park_id, dp.driver_id)
                         for p in parks
                         for dp in drivers_parks
                         if p.id == dp.park_id]
    
    many_to_many = [(d.fio, d.exp, park_name)
                    for park_name, park_id, driver_id in many_to_many_temp
                    for d in drivers if d.id == driver_id]

    print('Задание Б1')
    # Сортировка по водителям (индекс 0 - fio)
    res_11 = sorted(one_to_many, key=itemgetter(0))
    print(res_11)

    print('\nЗадание Б2')
    res_12_unsorted = []
    # Перебираем все автопарки
    for p in parks:
        # Список водителей автопарка
        p_drivers = list(filter(lambda i: i[2] == p.name, one_to_many))
        # Если автопарк не пустой
        if len(p_drivers) > 0:
            # Количество водителей в автопарке
            p_drivers_count = len(p_drivers)
            res_12_unsorted.append((p.name, p_drivers_count))
    
    # Сортировка по количеству водителей
    res_12 = sorted(res_12_unsorted, key=itemgetter(1))
    print(res_12)

    print('\nЗадание Б3')
    res_13 = {}
    # Перебираем всех водителей
    for d in drivers:
        if d.fio.endswith('ов'):
            # Список автопарков водителя
            d_parks = list(filter(lambda i: i[0] == d.fio, many_to_many))
            # Только названия автопарков
            d_parks_names = [x for _, _, x in d_parks]
            # Добавляем результат в словарь
            # ключ - водитель, значение - список автопарков
            res_13[d.fio] = d_parks_names

    print(res_13)

if __name__ == '__main__':
    main()