from colorama import Fore, init
from lab_python_oop import Rectangle, Circle, Square

init(autoreset=True)

N = 6  #Номер варианта

def main():
    rectangle = Rectangle(N, N, "синий")
    circle = Circle(N, "зеленый")
    square = Square(N, "красный")

    print(rectangle)
    print(circle)
    print(square)

    # Вызов метода внешнего пакета
    print(Fore.GREEN + "Проверка работы внешнего пакета colorama: OK")

if __name__ == "__main__":
    main()