import sys
import math


def get_coef(index, prompt):
    '''
    Читаем коэффициент из командной строки или вводим с клавиатуры
    с проверкой корректности значения.

    Args:
        index (int): Номер параметра в командной строке
        prompt (str): Приглашение для ввода коэффициента

    Returns:
        float: Коэффициент уравнения
    '''
    while True:
        try:
            # Пробуем прочитать коэффициент из командной строки
            coef_str = sys.argv[index]
        except IndexError:
            # Вводим с клавиатуры
            print(prompt)
            coef_str = input()

        # Пробуем перевести строку в действительное число
        try:
            coef = float(coef_str)
            return coef
        except ValueError:
            print('Некорректное значение коэффициента. Повторите ввод.')
            # Удаляем неверный аргумент из командной строки, чтобы при
            # следующей итерации запросить ввод с клавиатуры
            if index < len(sys.argv):
                sys.argv.pop(index)


def solve_square(a, b, c):
    '''
    Вычисление действительных корней квадратного уравнения at^2 + bt + c = 0

    Args:
        a (float): коэффициент A
        b (float): коэффициент B
        c (float): коэффициент C

    Returns:
        list[float]: Список действительных корней
    '''
    result = []
    D = b * b - 4 * a * c
    if D == 0.0:
        root = -b / (2.0 * a)
        result.append(root)
    elif D > 0.0:
        sqD = math.sqrt(D)
        root1 = (-b + sqD) / (2.0 * a)
        root2 = (-b - sqD) / (2.0 * a)
        result.append(root1)
        result.append(root2)
    return result


def get_roots(a, b, c):
    '''
    Вычисление действительных корней биквадратного уравнения
    Ax^4 + Bx^2 + C = 0

    Args:
        a (float): коэффициент А
        b (float): коэффициент B
        c (float): коэффициент C

    Returns:
        list[float]: Список действительных корней
    '''
    result = []

    # Если A == 0, уравнение не является биквадратным
    if a == 0.0:
        print('Коэффициент A не может быть равен нулю для биквадратного уравнения.')
        return result

    # Решаем квадратное уравнение относительно t = x^2
    t_roots = solve_square(a, b, c)

    # Для каждого t >= 0 находим x = ±sqrt(t)
    for t in t_roots:
        if t > 0.0:
            sqrt_t = math.sqrt(t)
            result.append(sqrt_t)
            result.append(-sqrt_t)
        elif t == 0.0:
            result.append(0.0)

    return result


def main():
    '''
    Основная функция
    '''
    a = get_coef(1, 'Введите коэффициент А:')
    b = get_coef(2, 'Введите коэффициент B:')
    c = get_coef(3, 'Введите коэффициент C:')

    # Вычисление корней
    roots = get_roots(a, b, c)

    # Вывод корней
    len_roots = len(roots)
    if len_roots == 0:
        print('Нет действительных корней')
    elif len_roots == 1:
        print('Один корень: {}'.format(roots[0]))
    elif len_roots == 2:
        print('Два корня: {} и {}'.format(roots[0], roots[1]))
    elif len_roots == 3:
        print('Три корня: {}, {} и {}'.format(roots[0], roots[1], roots[2]))
    elif len_roots == 4:
        print('Четыре корня: {}, {}, {} и {}'.format(
            roots[0], roots[1], roots[2], roots[3]))


# Если сценарий запущен из командной строки
if __name__ == "__main__":
    main()
