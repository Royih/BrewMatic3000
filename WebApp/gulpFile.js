var gulp = require('gulp');

// Include Our Plugins
var jshint = require('gulp-jshint');
var sass = require('gulp-sass');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');
var mainBowerFiles = require('main-bower-files');

// Lint Task
gulp.task('lint', function() {
    return gulp.src('js/*.js')
        .pipe(jshint())
        .pipe(jshint.reporter('default'));
});

// Compile Our Sass
gulp.task('sass', function() {
    return gulp.src('scss/*.scss')
        .pipe(sass())
        .pipe(gulp.dest('dist/css'));
});


// Concatenate & Minify JS
gulp.task('scripts', function() {
    return gulp.src('app/**/*.js')
        .pipe(concat('all.js'))
        .pipe(gulp.dest('wwwroot/assets'))
        .pipe(rename('all.min.js'))
        .pipe(uglify())
        .pipe(gulp.dest('wwwroot/assets'));
});

// Watch Files For Changes
gulp.task('watch', function() {
    gulp.watch('app/**/*.js', ['lint', 'scripts']);
    gulp.watch('app/**/*.html', ['html']);
    gulp.watch('scss/*.scss', ['sass']);
});

gulp.task('html', function(){
  // the base option sets the relative root for the set of files,
  // preserving the folder structure
  gulp.src(['app/**/*.html'], { base: './app/' })
  .pipe(gulp.dest('wwwroot/html'));
});

gulp.task('boostrap', function(){
  // the base option sets the relative root for the set of files,
  // preserving the folder structure
  gulp.src(['bower_components/bootstrap/dist/*/*.*'], { base: 'bower_components/bootstrap/dist/' })
  .pipe(gulp.dest('wwwroot/bootstrap'));
});

 
gulp.task('bower-files', function() {
    return gulp.src(mainBowerFiles())
        .pipe(gulp.dest("wwwroot/assets"));
});

// Default Task
gulp.task('default', ['lint', 'sass', 'bower-files','scripts','html','boostrap', 'watch']);