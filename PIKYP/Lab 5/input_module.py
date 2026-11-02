import sys

def get_coef(index, prompt):
    """
    Получение коэффициента из командной строки или с клавиатуры.
    """
    while True:
        try:
            coef_str = sys.argv[index]
        except IndexError:
            print(prompt)
            coef_str = input()

        try:
            coef = float(coef_str)
            return coef
        except ValueError:
            print('Некорректное значение коэффициента. Повторите ввод.')
            if index < len(sys.argv):
                sys.argv.pop(index)