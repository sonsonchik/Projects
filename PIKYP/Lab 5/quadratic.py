import math


def solve_square(a, b, c):
    """
    Вычисление действительных корней квадратного уравнения at^2 + bt + c = 0.

    Args:
        a (float): коэффициент A
        b (float): коэффициент B
        c (float): коэффициент C

    Returns:
        list[float]: Список действительных корней
    """
    if a == 0.0:
        raise ValueError('Коэффициент A не может быть равен нулю')

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
    """
    Вычисление действительных корней биквадратного уравнения
    Ax^4 + Bx^2 + C = 0.

    Args:
        a (float): коэффициент A
        b (float): коэффициент B
        c (float): коэффициент C

    Returns:
        list[float]: Список действительных корней
    """
    if a == 0.0:
        raise ValueError('Коэффициент A не может быть равен нулю для '
                         'биквадратного уравнения')

    result = []
    t_roots = solve_square(a, b, c)

    for t in t_roots:
        if t > 0.0:
            sqrt_t = math.sqrt(t)
            result.append(sqrt_t)
            result.append(-sqrt_t)
        elif t == 0.0:
            result.append(0.0)

    return result