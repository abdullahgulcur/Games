/*
Abdullah Gulcur

Game was run successfully by Microsoft Edge Browser 

If you encounter any problems, send and email address abdullahgulcur2@gmail.com 
*/

let Application = PIXI.Application,
    loader = PIXI.loader,
    resources = PIXI.loader.resources,
    TextureCache = PIXI.utils.TextureCache,
    Rectangle = PIXI.Rectangle,
    Text = PIXI.Text,
    TextStyle = PIXI.TextStyle,
    Sprite = PIXI.Sprite;

let app = new Application({
    width: 600,
    height: 600,
    antialias: true,
    transparent: false,
    backgroundColor: 001733,
    resolution: 1
}
);

document.body.appendChild(app.view);

loader
    .add("assets/sprites/player.png")
    .add("assets/sprites/enemyNew.png")
    .add("assets/sprites/lives.png")
    .add("assets/sprites/explosionn.png")
    .add("assets/sprites/bullet_0.png")
    .add("assets/sprites/bullet_1.png")
    .add("assets/sprites/star_0.png")
    .load(setup);


let player; /* Spaceship that we control*/

let state;  /* play, successfull, fail*/

/* Stars make parallax effect*/
let distantStars = [];
let closeStars = [];

let playerBullets = [];
let enemyBullets = [];

let enemies = []; /* Spaceships that attack us*/

let pressedSpace = false; /* Useful for when pressed space, there has to be instant bullet initialization */

var enemyHorizontalRange = 50; /* Enemy Spaceships move +- 50 horizontal range */

var firstPressed = true;

var frameCount = 0;

let scoreAmountText; /* Score text that player gets in level */

let highestScoreAmountText; /* The highest score that player have */

var totalScore = 0; /* Score amount that player gets in level */

var health = []; /* Player has 3 lives*/

var explosionVFX = [];

/* Each game end after 2 minutes*/
var gameTimer;
var levelPeriod = 120;
var timeText;

var gameState = "play";

var preserved = false; /* when player get hits, player would be preserved up to 3 seconds*/
var recoverTimer; /* recovery time for player when receive hits*/

let style0 = new TextStyle({
    fontFamily: "Arial",
    fontSize: 30,
    fontWeight: "bolder",
    fill: "red",
});

let style1 = new TextStyle({
    fontFamily: "Arial",
    fontSize: 25,
    fontWeight: "bolder",
    fill: "white",
});

let style2 = new TextStyle({
    fontFamily: "Arial",
    fontSize: 30,
    fontWeight: "bolder",
    fill: "red",
});

function loadScoreText() {

    var alignment = 10;

    var scoreText = new Text("SCORE", style0);
    scoreText.position.set(app.renderer.width - scoreText.width - alignment, alignment);
    app.stage.addChild(scoreText);

    scoreAmountText = new Text("000", style1);
    scoreAmountText.anchor.set(0.5);
    scoreAmountText.position.set(app.renderer.width - scoreText.width / 2 - alignment, alignment + scoreText.height + scoreAmountText.height / 2);
    app.stage.addChild(scoreAmountText);

    var highestScoreText0 = new Text("HIGHEST", style2);
    highestScoreText0.position.set(app.renderer.width - highestScoreText0.width - alignment, alignment * 2 + scoreAmountText.height + scoreText.height);
    app.stage.addChild(highestScoreText0);

    var highestScoreText1 = new Text("SCORE", style2);
    highestScoreText1.position.set(app.renderer.width - highestScoreText1.width - alignment, alignment * 5 + scoreAmountText.height + scoreText.height);
    app.stage.addChild(highestScoreText1);

    highestScoreAmountText = new Text("000", style1);
    highestScoreAmountText.anchor.set(0.5);
    highestScoreAmountText.position.set(app.renderer.width - scoreText.width / 2 - alignment, 160);
    app.stage.addChild(highestScoreAmountText);

}

function setPlayerHealthBar() {

    var totalHealth = 3;
    var alignment = 10;

    for (let i = 0; i < totalHealth; i++) {

        var healthIcon = new Sprite(TextureCache["assets/sprites/lives.png"]);
        healthIcon.scale.set(0.75);
        healthIcon.position.set((i+1) * (healthIcon.width + 10) - 30, alignment);
        health.push(healthIcon);
        app.stage.addChild(healthIcon);
    }
}

function animate() {

    window.requestAnimationFrame(animate);
    PIXI.timerManager.update();
}

function instantiateEnemy() {

    var enemy = new Enemy(new Sprite(TextureCache["assets/sprites/enemyNew.png"]), 1, randomInt(100, app.renderer.width - 100));
    enemy.sprite.anchor.set(0.5);
    enemy.sprite.scale.set(0.4, 0.4);
    enemy.sprite.rotation = Math.PI;
    enemy.sprite.position.set(enemy.startX, -enemy.sprite.height / 2);
    enemy.sprite.vx = 0;
    enemy.sprite.vy = 0;
    enemies.push(enemy);
    app.stage.addChild(enemy.sprite);
}

function setUpGameTimer() {

    gameTimer = PIXI.timerManager.createTimer(1000); // interval in milliseconds
    gameTimer.loop = true;

    gameTimer.on('start', function (elapsed) { setTimeText(); });
    gameTimer.on('end', function (elapsed) { console.log('end', elapsed) });
    gameTimer.on('repeat', function (elapsed, repeat) {

        /* Refresh timer */

        if (gameState === "play") {

            var leftMinutes = parseInt(levelPeriod / 60);
            var leftSeconds = levelPeriod % 60;

            if (leftSeconds <= 9)
                timeText.text = leftMinutes + ":0" + leftSeconds;
            else
                timeText.text = leftMinutes + ":" + leftSeconds;


            if (levelPeriod > 0)
                levelPeriod--;
            else {

                /* Game finishes */

                if (gameState === "play") {
                    destroyAllEnemies();
                    setSuccessfulText();
                    gameState = "successfull";

                    /* Refresh the highest score if last score is greater than the highest*/
                    if (totalScore > parseInt(highestScoreAmountText.text)) { 
                        highestScoreAmountText.text = totalScore;
                        write_cookie("score", totalScore);

                    }

                }
            }
        }
        else {/* Time stops */

            timeText.text = "0:00";
        }

    });

    gameTimer.start();

}

function setTimeText() {

    var alignment = 10;

    timeText = new Text("2:00", style1);
    timeText.position.set(app.renderer.width - timeText.width - alignment, app.renderer.height - timeText.height - alignment);
    app.stage.addChild(timeText);
}

function setup() {

    setUpGameTimer();

    setPlayerHealthBar();

    animate();

    /* Controls spacebar events */
    document.addEventListener('keydown', onKeyDown);
    document.addEventListener('keyup', onKeyUp);

    /* Loads stars */
    loadStarsFar(); 
    loadStarsClose();
    
    initiatePlayer();
    
    playerMovementController(); /* Controls max distances that player can go, and also sets player position when player reachs border of game*/

    state = play;

    app.ticker.add(delta => gameLoop(delta));

    loadScoreText();

    setHighestScoreAtFirst();

}

function setHighestScoreAtFirst() {

    if (read_cookie("score") === "" || read_cookie("score") === null)
        highestScoreAmountText.text = "000";
    else
        highestScoreAmountText.text = read_cookie("score");
}

function instantiateBulletFromPlayer() {

    soundEffect("assets/audios/shoot.wav", 0.03);

    let bulletLeft = new Sprite(TextureCache["assets/sprites/bullet_0.png"]);
    bulletLeft.anchor.set(0.5);
    bulletLeft.position.set(player.x - player.width * 0.35, player.y - player.height * 0.15);
    bulletLeft.scale.set(0.6, 0.6);
    playerBullets.push(bulletLeft);
    app.stage.addChild(bulletLeft);

    let bulletRight = new Sprite(TextureCache["assets/sprites/bullet_0.png"]);
    bulletRight.anchor.set(0.5);
    bulletRight.position.set(player.x + player.width * 0.35, player.y - player.height * 0.15);
    bulletRight.scale.set(0.6, 0.6);
    playerBullets.push(bulletRight);
    app.stage.addChild(bulletRight);


}

function instantiateBulletFromEnemy(enemy, delta) {

    let bulletLeft = new Sprite(TextureCache["assets/sprites/bullet_1.png"]);
    bulletLeft.anchor.set(0.5);
    bulletLeft.rotation = Math.PI;
    bulletLeft.position.set(enemy.x, enemy.y + enemy.height * 0.6);
    bulletLeft.scale.set(0.6, 0.6);
    enemyBullets.push(bulletLeft);
    app.stage.addChild(bulletLeft);
}

function destroyAllEnemies() {

    
    for (let i = 0; i < enemyBullets.length; i++) {

        startVFX(enemyBullets[i]);
        enemyBullets[i].visible = false;
        app.stage.removeChild(enemyBullets[i]);
    }
    
    for (let i = 0; i < enemies.length; i++) {

        startVFX(enemies[i].sprite);
        app.stage.removeChild(enemies[i].sprite);
    }

    
}

function setSuccessfulText() {

    let style = new TextStyle({
        fontFamily: "Arial",
        fontSize: 50,
        fontWeight: "bolder",
        fill: "red",
    });

    successfullText = new Text("SUCCESSFUL", style);
    successfullText.anchor.set(0.5);
    successfullText.position.set(app.renderer.width / 2, app.renderer.height / 2);
    app.stage.addChild(successfullText);

}

function play(delta) {

    player.x += player.vx;
    player.y += player.vy
}

function initiatePlayer() {

    player = new Sprite(TextureCache["assets/sprites/player.png"]);
    player.anchor.set(0.5);
    player.position.set(app.renderer.width / 2, app.renderer.height * 0.85)
    player.scale.set(0.4, 0.4);

    player.vx = 0;
    player.vy = 0;

    app.stage.addChild(player);
}

function enemyMovement(delta, index) {

    enemies[index].sprite.y += delta * 3;

    /* This if else block makes enemy horizontal movement */
    if (enemies[index].direction == 1) {

        if (enemies[index].sprite.x < enemies[index].startX + enemyHorizontalRange)
            enemies[index].sprite.x += delta * 2
        else
            enemies[index].direction = -1;
    } else {

        if (enemies[index].sprite.x > enemies[index].startX - enemyHorizontalRange)
            enemies[index].sprite.x -= delta * 2
        else
            enemies[index].direction = 1;
    }

    /* Enemy bullets are instantiated in every second from enemy */
    if(frameCount % (1 * 60) == 0 && gameState === "play")
        instantiateBulletFromEnemy(enemies[index].sprite, delta);

    /* This part has to be bottom of function because we cant reach enemies properties otherwise */
    if (enemies[index].sprite.y >= app.renderer.height + enemies[index].sprite.height / 2) {
        app.stage.removeChild(enemies[index].sprite);
        enemies.splice(index, 1);
    }
}

function setFailedText() {

    let style = new TextStyle({
        fontFamily: "Arial",
        fontSize: 50,
        fontWeight: "bolder",
        fill: "red",
    });

    failedText = new Text("GAME OVER !", style);
    failedText.anchor.set(0.5);
    failedText.position.set(app.renderer.width / 2, app.renderer.height / 2);
    app.stage.addChild(failedText);

}

function playerFails() {

    app.stage.removeChild(player);

    for (let i = 0; i < playerBullets.length; i++) {

        startVFX(playerBullets[i]);
        playerBullets[i].visible = false;
        app.stage.removeChild(playerBullets[i]);
    }
    
    if (totalScore > parseInt(highestScoreAmountText.text)) {
        highestScoreAmountText.text = totalScore;
        write_cookie("score", totalScore);

    }

}

/* Updates the highest score */
function write_cookie(name, value, path) {

    // Build the expiration date string:
    var expiration_date = new Date();
    expiration_date.setFullYear(expiration_date.getFullYear() + 1);
    expiration_date = expiration_date.toGMTString();

    // Build the set-cookie string:
    var cookie_string = escape(name) + "=" + escape(value) +
            "; expires=" + expiration_date;
    if (path != null)
        cookie_string += "; path=" + path;

    // Create/update the cookie:
    document.cookie = cookie_string;
}

/* Gets the highest score */
function read_cookie(key, skips) {
    // Set skips to 0 if parameter was omitted:
    if (skips == null)
        skips = 0;

    // Get cookie string and separate into individual cookie phrases:
    var cookie_string = "" + document.cookie;
    var cookie_array = cookie_string.split("; ");

    // Scan for desired cookie:
    for (var i = 0; i < cookie_array.length; ++i) {
        var single_cookie = cookie_array[i].split("=");
        if (single_cookie.length != 2)
            continue;
        var name = unescape(single_cookie[0]);
        var value = unescape(single_cookie[1]);

        // Return cookie if found:
        if (key == name && skips-- == 0)
            return value;
    }
    console.log("not found cookie");
    // Cookie was not found:
    return null;
}

function changePlayerPosition() {

    if (player.x < -player.width / 2)
        player.x = app.renderer.width + player.width / 2;

    if (player.x > app.renderer.width + player.width / 2)
        player.x = -player.width / 2

    if (player.y > app.renderer.height - player.height)
        player.y -= 5;

    if (player.y < 150)
        player.y += 5;
}

function gameLoop(delta) {

    changePlayerPosition();

    if (frameCount % 12 == 0 && pressedSpace && gameState !== "fail")
        instantiateBulletFromPlayer();

    if (frameCount % (100) == 0 && gameState !== "successfull")
        instantiateEnemy();

    if (gameState !== "fail") {

        for (let i = 0; i < distantStars.length; i++) {
            distantStars[i].y += delta;

            if (distantStars[i].y >= app.renderer.height) {
                distantStars[i].y -= app.renderer.height
            }
        }

        for (let i = 0; i < closeStars.length; i++) {
            closeStars[i].y += delta * 1.2;

            if (closeStars[i].y >= app.renderer.height) {
                closeStars[i].y -= app.renderer.height
            }
        }
    }

    

    for (let i = 0; i < enemies.length; i++) {

        enemyMovement(delta, i);
        
        /*
        if (hitTestRectangle(player, enemies[i].sprite) && gameState !== "fail") { // enemy hits player

            if (!preserved && gameState === "play") {
                recoverTimer = PIXI.timerManager.createTimer(3000);

                recoverTimer.on('start', function (elapsed) { preserved = true; });
                recoverTimer.on('end', function (elapsed) { preserved = false; console.log("finished"); });
                recoverTimer.on('repeat', function (elapsed, repeat) { });
                recoverTimer.start();

                startVFX(enemies[i].sprite);
                soundEffect("assets/audios/explosion.wav", 0.2);

                if (health.length > 1) {
                    app.stage.removeChild(health[health.length - 1]);
                    health.pop();
                }
                else {
                    app.stage.removeChild(health[0]);
                    health.pop();
                    gameState = "fail";
                    setFailedText();
                    playerFails();
                }

                enemies[i].sprite.visible = false;
                app.stage.removeChild(enemies[i].sprite);
                enemies.splice(i, 1);
                break;

            }

        }*/
    }

    for (let i = 0; i < playerBullets.length; i++) {

        playerBullets[i].y -= delta * 8;
        onPlayerBulletEnterEnemy(playerBullets[i], delta); /* Checks whether any of player bullet hit enemy in every frame */

        if (playerBullets[i].y < -playerBullets[i].height) {

            app.stage.removeChild(playerBullets[i]);
            playerBullets.splice(i, 1);
            break;
        }
    }

    for (let i = 0; i < enemyBullets.length; i++) {

        enemyBullets[i].y += delta * 8;

        if (hitTestRectangle(player, enemyBullets[i]) && gameState !== "fail") { // enemy bullet hits player

            /* Time for recovery starts */
            if (!preserved && gameState === "play") {
                recoverTimer = PIXI.timerManager.createTimer(3000);

                recoverTimer.on('start', function (elapsed) { preserved = true; });
                recoverTimer.on('end', function (elapsed) { preserved = false; console.log("finished");});
                recoverTimer.on('repeat', function (elapsed, repeat) { });
                recoverTimer.start();

                startVFX(player);
                soundEffect("assets/audios/explosion.wav", 0.2);

                if (health.length > 1) {
                    app.stage.removeChild(health[health.length - 1]);
                    health.pop();
                }
                else {
                    app.stage.removeChild(health[0]);
                    health.pop();
                    gameState = "fail";
                    setFailedText();
                    playerFails();
                }

                enemyBullets[i].visible = false;
                app.stage.removeChild(enemyBullets[i]);
                enemyBullets.splice(i, 1);
                break;

            }

        }

        /* If enemy bullets exceed border of canvas, they would be destroyed immediately*/
        if (enemyBullets[i].y > app.renderer.height + enemyBullets[i].height) {

            app.stage.removeChild(enemyBullets[i]);
            enemyBullets.splice(i, 1);
            break;
        }
        
    }

    /* Animation for indicate player is preserved*/
    if (preserved) {

        if (frameCount % 5 == 0) {

            if (player.visible == true) {
                player.visible = false;
            }
            else {
                player.visible = true;
            }
        }
    }
    else
        player.visible = true;
    

    state(delta);

    frameCount++;
}

function soundEffect(path, vol) {

    var sfx = new Audio();
    sfx.src = path;
    sfx.volume = vol;
    sfx.play();
}

function onPlayerBulletEnterEnemy(bullet, delta) { // player bullet hits enemy

    //Define the variables we'll need to calculate
    let hit, combinedHalfWidths, combinedHalfHeights, vx, vy;

    //hit will determine whether there's a collision
    hit = false;

    for (let i = 0; i < enemies.length; i++) {

        bullet.centerX = bullet.x;// + r1.width / 2;
        bullet.centerY = bullet.y;// + r1.height / 2;

        enemies[i].sprite.centerX = enemies[i].sprite.x;// + r2.width / 2;
        enemies[i].sprite.centerY = enemies[i].sprite.y;// + r2.height / 2;

        //Find the half-widths and half-heights of each sprite
        bullet.halfWidth = bullet.width / 2;
        bullet.halfHeight = bullet.height / 2;
        enemies[i].sprite.halfWidth = enemies[i].sprite.width / 2;
        enemies[i].sprite.halfHeight = enemies[i].sprite.height / 2;
        //Calculate the distance vector between the sprites
        vx = bullet.centerX - enemies[i].sprite.centerX;
        vy = bullet.centerY - enemies[i].sprite.centerY;

        //Figure out the combined half-widths and half-heights
        combinedHalfWidths = bullet.halfWidth + enemies[i].sprite.halfWidth;
        combinedHalfHeights = bullet.halfHeight + enemies[i].sprite.halfHeight;


        //Check for a collision on the x axis
        if (Math.abs(vx) < combinedHalfWidths * 0.9) {

            //A collision might be occurring. Check for a collision on the y axis
            if (Math.abs(vy) < combinedHalfHeights * 0.5) {

                //There's definitely a collision happening
                hit = true;
            } else {

                //There's no collision on the y axis
                hit = false;
            }
        } else {

            //There's no collision on the x axis
            hit = false;
        }

        if (hit) {

            startVFX(enemies[i].sprite);
            soundEffect("assets/audios/explosion.wav", 0.3);

            totalScore += 1000;
            
            if (totalScore > parseInt(highestScoreAmountText.text)) {
                highestScoreAmountText.text = totalScore;
                write_cookie("score", totalScore);
            }

            scoreAmountText.text = totalScore;
            bullet.visible = false;

            app.stage.removeChild(enemies[i].sprite);
            enemies.splice(i, 1);
            break;
        }
    }

}

/* Explosion virtual effect */
function startVFX(startPoint) {

    var mySpriteSheetImage = PIXI.BaseTexture.fromImage("assets/sprites/explosionn.png");
    var rect = new PIXI.Rectangle(0, 0, 157, 157)
    var spriteTexture0 = new PIXI.Texture(mySpriteSheetImage, rect);

    var sprite = new Sprite(spriteTexture0);
    sprite.anchor.set(0.5);
    sprite.position.set(startPoint.x, startPoint.y);

    var explosion = setInterval(function () {

        if (rect.x >= 157 * 9)
            app.stage.removeChild(sprite);
        else {
            sprite.texture.frame = rect;
            rect.x += 157;
        }
        

    }, 50);

    app.stage.addChild(sprite);
}

function onKeyDown(key) {

    if (key.keyCode === 32) { // space
        pressedSpace = true;

        if (firstPressed && gameState !== "fail") {
            instantiateBulletFromPlayer();
            firstPressed = false;
        }
    }
}

function onKeyUp(key) {

    if (key.keyCode === 32) { // space
        pressedSpace = false;
        firstPressed = true;
    }
}

/* Controls player horizontal and vertical movement */
function playerMovementController() {

    let left = keyboard("ArrowLeft"),
      up = keyboard("ArrowUp"),
      right = keyboard("ArrowRight"),
      down = keyboard("ArrowDown");

    //Left arrow key `press` method
    left.press = () => {

        player.vx = -5;
    };

    //Left arrow key `release` method
    left.release = () => {

        if (!right.isDown) {
            player.vx = 0;
        }
    };

    //Up
    up.press = () => {

        player.vy = -5;
            
    };
    up.release = () => {
        if (!down.isDown) {
            player.vy = 0;
        }
    };

    //Right
    right.press = () => {
        player.vx = 5;
    };
    right.release = () => {

        if (!left.isDown) {
            player.vx = 0;
        }
    };

    //Down
    down.press = () => {

        player.vy = 5;

    };
    down.release = () => {
        if (!up.isDown) {
            player.vy = 0;
        }
    };

}

function keyboard(value) {
    let key = {};
    key.value = value;
    key.isDown = false;
    key.isUp = true;
    key.press = undefined;
    key.release = undefined;
    //The `downHandler`
    key.downHandler = event => {
        if (event.key === key.value) {
            if (key.isUp && key.press) key.press();
            key.isDown = true;
            key.isUp = false;
            event.preventDefault();
        }
    };

    //The `upHandler`
    key.upHandler = event => {
        if (event.key === key.value) {
            if (key.isDown && key.release) key.release();
            key.isDown = false;
            key.isUp = true;
            event.preventDefault();
        }
    };

    //Attach event listeners
    const downListener = key.downHandler.bind(key);
    const upListener = key.upHandler.bind(key);

    window.addEventListener(
      "keydown", downListener, false
    );
    window.addEventListener(
      "keyup", upListener, false
    );

    // Detach event listeners
    key.unsubscribe = () => {
        window.removeEventListener("keydown", downListener);
        window.removeEventListener("keyup", upListener);
    };

    return key;
}

/* Fills canvas with stars according to specific algorithm */
function loadStarsFar() {

    let numberOfStarsForEachLine = 6;

    let spacing = parseInt(app.renderer.width / numberOfStarsForEachLine);
    let clutter = 2;
      
    for (let i = 0; i < numberOfStarsForEachLine; i++) {

        for (let j = 0; j < numberOfStarsForEachLine; j++) {

            let star = new Sprite(TextureCache["assets/sprites/star_0.png"]);
            star.anchor.set(0.5);
            star.scale.set(0.05);
            star.position.set(parseInt(spacing * j + spacing / clutter + randomInt(-spacing / clutter, spacing / clutter)),
                parseInt(spacing * i + spacing / clutter) + parseInt(randomInt(-spacing / clutter, spacing / clutter)));

            distantStars.push(star);
            app.stage.addChild(star);
        }
    }
}

function loadStarsClose() {

    let numberOfStarsForEachLine = 4;

    let spacing = parseInt(app.renderer.width / numberOfStarsForEachLine);
    let clutter = 2;

    for (let i = 0; i < numberOfStarsForEachLine; i++) {

        for (let j = 0; j < numberOfStarsForEachLine; j++) {

            let star = new Sprite(TextureCache["assets/sprites/star_0.png"]);
            star.anchor.set(0.5);
            star.scale.set(0.08);
            star.position.set(parseInt(spacing * j + spacing / clutter + randomInt(-spacing / clutter, spacing / clutter)),
                parseInt(spacing * i + spacing / clutter) + parseInt(randomInt(-spacing / clutter, spacing / clutter)));

            closeStars.push(star);
            app.stage.addChild(star);
        }
    }
}

function randomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

function hitTestRectangle(r1, r2) {

    //Define the variables we'll need to calculate
    let hit, combinedHalfWidths, combinedHalfHeights, vx, vy;

    //hit will determine whether there's a collision
    hit = false;

    r1.centerX = r1.x;// + r1.width / 2;
    r1.centerY = r1.y;// + r1.height / 2;
    
    r2.centerX = r2.x;// + r2.width / 2;
    r2.centerY = r2.y;// + r2.height / 2;
    
    //Find the half-widths and half-heights of each sprite
    r1.halfWidth = r1.width / 2;
    r1.halfHeight = r1.height / 2;
    r2.halfWidth = r2.width / 2;
    r2.halfHeight = r2.height / 2;
    //Calculate the distance vector between the sprites
    vx = r1.centerX - r2.centerX;
    vy = r1.centerY - r2.centerY;

    //Figure out the combined half-widths and half-heights
    combinedHalfWidths = r1.halfWidth + r2.halfWidth;
    combinedHalfHeights = r1.halfHeight + r2.halfHeight;


    //Check for a collision on the x axis
    if (Math.abs(vx) < combinedHalfWidths * 0.9) {

        //A collision might be occurring. Check for a collision on the y axis
        if (Math.abs(vy) < combinedHalfHeights * 0.1) {

            //There's definitely a collision happening
            hit = true;
        } else {

            //There's no collision on the y axis
            hit = false;
        }
    } else {

        //There's no collision on the x axis
        hit = false;
    }

    //`hit` will be either `true` or `false`
    return hit;
}

class Enemy{

    constructor(sprite, direction, startX) {
        this.sprite = sprite;
        this.direction = direction;
        this.startX = startX;

    }

}

