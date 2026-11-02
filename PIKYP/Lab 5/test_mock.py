import unittest
from unittest.mock import patch
import sys
from input_module import get_coef


class TestGetCoefMock(unittest.TestCase):
    """Тесты с использованием Mock-объектов."""

    @patch('sys.argv', ['prog.py', '3.5'])
    def test_from_command_line(self):
        """Коэффициент корректно читается из командной строки."""
        # Так как в sys.argv есть '3.5', программа не должна просить ввод с клавиатуры
        result = get_coef(1, 'Введите A:')
        self.assertEqual(result, 3.5)

    @patch('builtins.input', return_value='2.5')
    def test_from_keyboard(self, mock_input):
        """Коэффициент читается с клавиатуры, если нет аргументов."""
        # Очищаем sys.argv (оставляем только название скрипта)
        with patch('sys.argv', ['prog.py']):
            result = get_coef(1, 'Введите A:')
            
        self.assertEqual(result, 2.5)
        # Проверяем, что функция input() действительно вызывалась 1 раз
        mock_input.assert_called_once()

    @patch('builtins.input', side_effect=['abc', '7'])
    @patch('sys.argv', ['prog.py'])
    def test_repeat_on_invalid(self, mock_input):
        """При некорректном вводе запрос повторяется."""
        # side_effect=['abc', '7'] означает: первый вызов input() вернет 'abc', второй - '7'
        result = get_coef(1, 'Введите A:')
        
        self.assertEqual(result, 7.0)
        # Проверяем, что input() вызывался 2 раза (сначала ошибка, потом успех)
        self.assertEqual(mock_input.call_count, 2)


if __name__ == '__main__':
    unittest.main()