class Color:
    """Цвет фигуры."""
    def __init__(self, color):
        self._color = color

    @property
    def color(self):
        return self._color

    @color.setter
    def color(self, value):
        self._color = value

    def __repr__(self):
        return self._color