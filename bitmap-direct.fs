\ Direct framebuffer bitmap loader for Crazytown
\ Uses memory copy for fast display

decimal

\ Get framebuffer address directly
: get-fb-addr ( -- addr )
  " screen" open-dev ?dup if
    >r
    " frame-buffer-adr" r@ $call-method
    r> close-dev
  else
    0
  then
;

\ Variables for framebuffer
variable fb-addr
variable fb-stride   \ bytes per line

\ Initialize framebuffer access
: init-fb ( -- )
  get-fb-addr fb-addr !
  1280 4 * fb-stride !  \ 4 bytes per pixel for 32-bit color
  
  fb-addr @ 0= if
    ." Failed to get framebuffer!" cr
    abort
  then
;

\ Convert 24-bit BGR to 32-bit ARGB for framebuffer
: bgr>argb ( b g r -- argb )
  16 lshift swap 8 lshift or or
  ff000000 or  \ Add alpha = 0xFF
;

\ Fast memory copy for one line of pixels
: copy-bmp-line ( src-addr dst-addr width -- )
  0 ?do
    \ Read BGR from source
    over i 3 * + dup c@        \ B
    over 1+ c@                  \ G
    rot 2 + c@                  \ R
    
    \ Convert and store to framebuffer
    bgr>argb
    2 pick i 4 * + !           \ Store 32-bit value
  loop
  2drop
;

\ Direct BMP to framebuffer copy
: bmp>fb ( -- )
  cr ." Direct framebuffer copy..." cr
  
  \ Load BMP file
  " hd:\rd_boot.bmp" " load" evaluate
  
  load-size 0= if
    ." File not found!" cr exit
  then
  
  \ Simple validation
  load-base c@ char B <> 
  load-base 1+ c@ char M <> or if
    ." Not a BMP!" cr exit
  then
  
  init-fb
  
  \ BMP is bottom-up, so we start from bottom
  854 0 ?do
    \ Source: BMP pixel data (skip 54-byte header)
    load-base 54 +              \ Start of pixel data
    854 i - 1- 3840 * +         \ Row (3840 = 1280 * 3)
    
    \ Destination: framebuffer
    fb-addr @                   \ Framebuffer start
    i fb-stride @ * +           \ Row offset
    
    \ Copy entire line
    1280 copy-bmp-line
    
    \ Progress
    i 50 mod 0= if ." ." then
  loop
  
  cr ." Done!" cr
;

\ Even faster: use move if pixels align
: bmp>fb-fast ( -- )
  cr ." Ultra-fast framebuffer copy..." cr
  
  " hd:\rd_boot.bmp" " load" evaluate
  load-size 0= if exit then
  
  init-fb
  
  \ If BMP is 32-bit ARGB already, we can use move
  load-base 28 + c@ 32 = if
    ." 32-bit BMP - direct copy!" cr
    
    \ Copy all pixels at once (flipping vertically)
    854 0 ?do
      load-base 54 + 
      854 i - 1- 5120 * +       \ 5120 = 1280 * 4
      
      fb-addr @ i fb-stride @ * +
      
      5120 move                 \ Copy entire row
    loop
  else
    ." 24-bit BMP - use bmp>fb instead" cr
    bmp>fb
  then
;

\ Test direct framebuffer
: test-fb ( -- )
  init-fb
  ." Framebuffer at: " fb-addr @ . cr
  
  \ Fill screen with test pattern
  fb-addr @ 1280 854 * 4 * ff0000ff fill
  
  ." Screen should be red!" cr
;

decimal