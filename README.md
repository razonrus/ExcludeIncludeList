# ExcludeIncludeList

Класс, позволяющий создавать и оперировать со списками, содержащими включенные или исключенные объекты.

Например, нам нужно реализовать фильтр по цветам. Варианты фильтра:
  - все доступные цвета кроме синего и розового
  - все доступные цвета
  - только желтый и зеленый
  - ни один из доступных цветов
  
при этом доступных цветов может быть очень много, хранить все разрешенные/все запрещенные в объекте может быть нецелесообразно.

Поддерживаемые функции:
  - объединение включающих/исключающих списков (Concat)
  - пересечение включающих/исключающих списков (Intersect)
  - проверка на принадлежность элемента списку (Contains)
  - преобразование списка (Select)
  - получение включенных элементов по полному набору значений (GetAllItems)