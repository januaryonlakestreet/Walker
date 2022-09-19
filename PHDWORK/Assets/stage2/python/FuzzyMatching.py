import base64


class FuzzyMatch:

    def __init__(self, Situation):
        self._Situation = Situation

    def GetResult(self):
        # load list of animations
        # cycle through each frame
        # find the smallest hamming distance
        # 'base 64 it' and return frame.
        return "Result"

    def GetDistance(self, a):
        bstring = self._Situation
        astring = a
        StringLength = len(self._Situation)
        Distance = 0

        Distance += abs(astring - bstring)

        for x in range(StringLength):
            if bstring[x] != astring[x]:
                Distance += 1

        return Distance
        # https://en.wikipedia.org/wiki/Hamming_distance
