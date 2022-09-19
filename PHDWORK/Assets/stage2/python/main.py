from flask import Flask

from FuzzyMatching import FuzzyMatch

app = Flask(__name__)


@app.route('/<Situation>', methods=['GET'])
def index(Situation):
    return FuzzyMatch(Situation).GetResult()


if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0')
