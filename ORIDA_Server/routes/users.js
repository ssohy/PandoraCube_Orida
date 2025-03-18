const express = require('express');
const router = express.Router();
const client = require('../db/db'); // MySQL 연결 클라이언트
const bcrypt = require('bcrypt');
const jwt = require('jsonwebtoken');
const multer = require('multer');
const upload = multer(); // 파일 저장 없이 `multipart/form-data`만 처리
const USER_COOKIE_KEY = 'USER';

async function fetchUser(user_id) {
    return new Promise((resolve, reject) => {
        client.query('SELECT * FROM users WHERE user_id = ?', [user_id], (err, results) => {
            if (err) {
                return reject(err);
            }
            resolve(results[0]);
        });
    });
}

async function createUser(newUser) {
    return new Promise(async (resolve, reject) => {
        try {
            if (!newUser.user_pw) {
                return reject(new Error('비밀번호가 제공되지 않았습니다.'));
            }

            console.log('Hashing password:', newUser.user_pw); // 디버깅용 로그 추가
            newUser.user_pw = await bcrypt.hash(newUser.user_pw, 10);

            client.query(
                'INSERT INTO users (user_id, user_pw, user_nickname) VALUES (?, ?, ?)',
                [newUser.user_id, newUser.user_pw, newUser.user_nickname],
                (err, results) => {
                    if (err) {
                        return reject(err);
                    }
                    resolve(results);
                }
            );
        } catch (error) {
            reject(error);
        }
    });
}


// 회원 가입 라우트
router.post('/signup', upload.none(), async (req, res) => {
    console.log('🟢 요청이 들어옴!');
    console.log('Headers:', req.headers); // 요청 헤더 확인
    console.log('Content-Type:', req.get('Content-Type')); // Content-Type 확인
    console.log('Received request body:', req.body); // 요청 본문 확인

    const { user_id, user_pw, user_nickname } = req.body;

    if (!user_id || !user_pw || !user_nickname) {
        return res.status(400).json({ msg: '모든 필드를 입력해야 합니다.' });
    }

    try {
        let user = await fetchUser(user_id);
        if (user) {
            return res.status(400).json({ msg: 'User already exists' });
        }

        const newUser = { user_id, user_pw, user_nickname };

        await createUser(newUser);

        res.json({ msg: '회원가입 성공' });
    } catch (err) {
        console.error(err.message);
        res.status(500).send('Server error');
    }
});


// 로그인 라우트
router.post('/signin', async (req, res) => {
    const { user_id, user_pw } = req.body;

    try {
        let user = await fetchUser(user_id);
        if (!user) {
            return res.status(400).json({ msg: '사용자를 찾을 수 없습니다.' });
        }

        console.log('User found:', user);
        console.log('Comparing password:', user_pw, 'with hash:', user.user_pw);

        const isMatch = await bcrypt.compare(user_pw, user.user_pw);
        console.log('Password match result:', isMatch); // 디버깅 로그 추가
        if (!isMatch) {
            console.log('Password does not match');
            return res.status(400).json({ msg: '비밀번호 오류' });
        }

        const payload = { user: { user_id: user.user_id } };
        jwt.sign(payload, 'secret', { expiresIn: 360000 }, (err, token) => {
            if (err) throw err;
            res.cookie(USER_COOKIE_KEY, JSON.stringify(payload.user));
            res.redirect('/'); // 로그인 성공 시 홈 페이지로 리디렉션
        });
    } catch (err) {
        console.error(err.message);
        res.status(500).send('Server error');
    }
});

// 사용자 정보 확인 라우트
router.get('/', async (req, res) => {
    const userCookie = req.cookies[USER_COOKIE_KEY];

    if (userCookie) {
        const userData = JSON.parse(userCookie);
        const user = await fetchUser(userData.user_id);
        if (user) {
            res.status(200).send(`
                <a href="/logout">Log Out</a>
                <h1>id: ${user.user_id}, nickname: ${user.user_nickname}</h1>
            `);
            return;
        }
    }

    res.status(200).send(`
        <a href="/signin.html">Sign In</a>
        <a href="/signup.html">Sign Up</a>
        <h1>Not Logged In</h1>
    `);
});

module.exports = router;