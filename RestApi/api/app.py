from flask import Flask, jsonify, request
from flask_sqlalchemy import SQLAlchemy
import io
import sys
import os
from scipy.io import wavfile as wav
import numpy as np
from werkzeug.utils import secure_filename
from detect_phoneme import get_score

app = Flask(__name__)

app.config['SQLALCHEMY_DATABASE_URI'] = 'postgres://ymkzuxwlkldyab:f9043b3528a1e5d428e5cccddf298176619c9a82400227aec156794a28a481b0@ec2-46-137-91-216.eu-west-1.compute.amazonaws.com:5432/d6ddg8pa2phsko'
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = True
app.config['UPLOAD_FOLDER'] = ""

db = SQLAlchemy(app)

@app.route('/')
def hello_world():
   return 'Explora Tablet API'

#Login function
@app.route('/list')
def fetch_data():
    args = request.args
    fetch_result = userlist.query.all()
    print(fetch_result)
    if "user_name" in args:
        u_name = args["user_name"]
        fetch_result = userlist.query.filter(userlist.user_name == u_name).all()
        if(fetch_result): 
            return(fetch_result[0].toJSON())
        else:
            return("USER NOT FOUND")
    else:
       fetch_result = userlist.query.all()
       #print(fetch_result)
    print(jsonify([str(result) for result in fetch_result]))
    return jsonify(users = [str(result) for result in fetch_result])

#Leaderboard function
@app.route('/leaderboard')
def leaderboard():
    args = request.args
    #u_name = args["user_name"]
    fetch_result_e = fetch_result = userlist.query.filter(userlist.avatar % 3 == 0).all()
    fetch_result_a = fetch_result = userlist.query.filter(userlist.avatar % 3 == 1).all()
    fetch_result_w = fetch_result = userlist.query.filter(userlist.avatar % 3 == 2).all()
    result0 = sum([x.score for x in fetch_result_e])
    result1 = sum([x.score for x in fetch_result_a])
    result2 = sum([x.score for x in fetch_result_w])
    return(jsonify(escore = result0, ascore = result1, wscore = result2));

    """if(fetch_result_e and fetch_result_a and fetch_result_e): 
        resilt
    	return(fetch_result[0].toJSON())
    else:
    	return("ERROR")"""

@app.route('/uploadfile',methods=['GET','POST'])
def uploadfile():

    """if request.method == 'POST':
        file = request.files['file']
        if file and allowed_file(file.filename):
            filename = secure_filename(file.filename)
            s3 = boto.connect_s3()
            bucket = s3.create_bucket('my_bucket')
            key = bucket.new_key(filename)
            key.set_contents_from_file(file, headers=None, replace=True, cb=None, num_cb=10, policy=None, md5=None) 
            return 'successful upload'
    """
    print("uploading file...",request.files['file'])
    args = request.args
    print(args["user_name"])
    print(args["current_stimuli"])
    if request.method == 'POST':
        f = request.files['file']
        binary = f.read()
        #filePath = "audios/" +  args["current_stimuli"] + args["user_name"] + secure_filename(f.filename)
        #filePath = os.path.join(app.config['UPLOAD_FOLDER'], filePath)
        #wav.write(filePath,16000,np.array(f.stream.read()))
        #sprt, data = wav.read(filePath)
        print("Saving ",f)
        print("Type ",io.BytesIO(binary))
        #return("P00")
        
        score, phonemes = get_score(io.BytesIO(binary), int(args["current_stimuli"][1] + args["current_stimuli"][2]))
        print(score)
        return("P" + str(len(phonemes)) + str(score)+ ''.join(phonemes))


#Register function
@app.route('/list_add', methods=['POST'])
def insert_user():
  
    data = request.get_json()
    #print(data)
    sys.stdout.flush()
    new_user_name = data['user_name']
    new_score = data['score']
    new_mode = data['mode']
    new_avatar = data['avatar']
    new_bag = data['bag']

    incompat_users = userlist.query.filter(userlist.user_name == new_user_name).all()
    #print(incompat_users)
    if(incompat_users):
    	return "Username Alredy in use"
    else:
        user = userlist(new_user_name, 1, new_score, new_mode, new_avatar, new_bag)
        db.session.add(user)
        db.session.commit()
    return "OK"

#Save progress function
@app.route('/list_save', methods=['POST'])
def save_user_progress():
    
    data = request.get_json()
    print(data)
    sys.stdout.flush()
    new_user_name = data['user_name']
    new_score = data['score']
    new_lvl = data['lvl']
    new_bag = data['bag']
    new_history = data['stimuli_history']
    new_answers = data['answers']
    users = userlist.query.filter(userlist.user_name == new_user_name).all()
    #print(incompat_users)
    if(not users):
   	    return "Error while saving data"
    else:
        user = userlist.query.filter(userlist.user_name == new_user_name).update(dict(score=new_score,lvl = new_lvl,bag = new_bag, stimuli_history = new_history, answers = new_answers))
	    #user = userlist(new_user_name, new_score)
	    #db.session.add(user)
        db.session.commit()
    return "OK"
      
class userlist(db.Model):
    user_id = db.Column(db.Integer, primary_key=True, nullable=False, autoincrement=True)
    user_name = db.Column(db.String(80), nullable=False)
    lvl = db.Column(db.Integer, unique=False, nullable=True)
    score = db.Column(db.Integer, unique=False, nullable=True)
    mode = db.Column(db.Integer, unique=False, nullable=True)
    avatar = db.Column(db.Integer, nullable=False)
    bag = db.Column(db.String(8*8), nullable=False)
    stimuli_history = db.Column(db.String(1500), nullable=False)
    answers = db.Column(db.String(4300), nullable=False)
    
    #utype = db.Column(db.String(80), nullable=True)
    #mode = db.Column(db.Integer, unique=False, nullable=True)

    def __init__(self, new_user_name, new_lvl, new_score, new_mode, new_avatar, new_bag):
        self.user_name = new_user_name
        self.lvl = new_lvl
        self.score = new_score
        self.mode = new_mode
        self.avatar = new_avatar
        self.bag = new_bag
        self.stimuli_history = ""
        self.answers = ""
     
    def __repr__(self):
    	#print(str())
    	#return str(jsonify(userId = self.user_id,userName=self.user_name, score = self.score))
        return '{' + '"user_id": {0}, "user_name": "{1}", "lvl": {2}, "score" : {3}, "mode" : {4}, "avatar" : {5}, "bag" : "{6}",  "stimuli_history" : "{7}",  "answers" : "{8}"'.format(self.user_id, self.user_name, self.lvl, self.score, self.mode, self.avatar, self.bag, self.stimuli_history, self.answers) + '}'
        
    def toJSON(self):
    	return jsonify(user_id = self.user_id,
                        user_name=self.user_name,
                        score = self.score,
                        lvl = self.lvl,
                        mode = self.mode,
                        avatar = self.avatar,
                        bag = self.bag,
                        stimuli_history = self.stimuli_history,
                        answers = self.answers)

if __name__ == "__main__":
    app.run(threaded=True, debug=False, port=5432)
