const express = require('express');
const path = require('path');
const usersRouter = require('./routes/users');
const app = express();

// 설정
app.set('view engine', 'ejs');
app.set('views', path.join(__dirname, 'views'));

// 라우트 설정
app.use('/api/users', usersRouter);

// 뷰 라우트 설정
app.get('/', (req, res) => {
    res.render('signin', { title: 'Sign In' });
});

app.get('/signup', (req, res) => {
    res.render('signup', { title: 'Sign Up' });
});

app.get('/signin', (req, res) => {
    res.render('signin', { title: 'Sign In' });
});

// 로그아웃 라우트
app.post('/logout', (req, res) => {
    res.redirect('/signin');
});

// 서버 시작
const PORT = process.env.PORT || 5006;
app.listen(PORT, () => console.log(`Server running on  http://localhost:${PORT}`));
