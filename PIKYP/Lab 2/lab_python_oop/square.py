from .rectangle import Rectangle

class Square(Rectangle):
    """Квадрат — частный случай прямоугольника."""
    FIGURE_NAME = "Квадрат"

    def __init__(self, side, color):
        super().__init__(side, side, color)
        self.side = side

    def get_name(self):
        return self.FIGURE_NAME

    def __repr__(self):
        return "{}: сторона={}, цвет={}, площадь={:.2f}".format(
            self.get_name(), self.side,
            self.color.color, self.area()
        )