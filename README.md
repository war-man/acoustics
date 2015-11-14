# Acoustics
A collection of acoustic utilities written in C#.

## Modal Analysis

Performs a modal analysis on an empty room, assuming flat walls.

#### Method

1. Calculate the critical frequency of the given room:
 * Calculate the volume of the room.
 * Calculate the surface area of the room (4 walls + roof + floor, flat planes).
 * Calculate the mean free path (MFP).
 * Calculate the critical frequency.
2. Calculate the fundamental axial modes for each axis
 * f = v/2D
 * Where D = Axis length (m) and v = Speed of sound (m/s)
3. Calculate axial mode harmonics up to critical frequency
4. Sort harmonics for all axes
5. Find coincidences
6. Find spacings

#### Usage

| Argument | Description    | Type   | Unit  | Optional             |
|----------|----------------|--------|-------|----------------------|
| -l       | Room length    | Double | Hertz | No                   |
| -w       | Room width     | Double | Hertz | No                   |
| -h       | Room height    | Double | Hertz | No                   |
| -v       | Speed of sound | Double | m/s   | Yes (default 344m/s) |
