\ Crazytown Fast - Lock Screen with direct framebuffer access
\ Optimized version for PowerBook G4

decimal

\ Open display device
0 value myscreen
" screen" open-dev to myscreen

\ Screen constants
1280 constant screen-width
854 constant screen-height

\ Get framebuffer for direct access
0 value fb-base
0 value fb-stride

: init-fast-display ( -- )
  myscreen 0= if
    ." Failed to open screen!" cr abort
  then
  
  \ Get framebuffer address
  " frame-buffer-adr" myscreen $call-method to fb-base
  screen-width 4 * to fb-stride
  
  fb-base 0= if
    ." No framebuffer!" cr abort
  then
;

\ Fast BMP loader using direct memory access
: load-bmp-fast ( -- )
  " hd:\rd_boot.bmp" " load" evaluate
  
  load-size 0= if
    ." No bitmap found" cr exit
  then
  
  \ Verify BMP
  load-base dup c@ char B = 
  swap 1+ c@ char M = and 0= if
    ." Invalid BMP" cr exit
  then
  
  \ Get data offset (usually 54)
  load-base 10 + @ to load-offset
  
  \ Copy pixels line by line (BMP is bottom-up)
  screen-height 0 ?do
    \ Calculate addresses
    load-base load-offset +
    screen-height i - 1- screen-width 3 * * +  \ Source line
    
    fb-base i fb-stride * +                     \ Dest line
    
    \ Convert BGR to ARGB line by line
    screen-width 0 ?do
      \ Read BGR
      over j 3 * + dup c@       \ B
      over 1+ c@                \ G  
      rot 2 + c@                \ R
      
      \ Write ARGB
      ff000000 or swap 8 lshift or swap 16 lshift or
      over j 4 * + !
    loop
    2drop
    
    \ Progress every 50 lines
    i 50 mod 0= if ." ." then
  loop
  cr
;

\ Password handling
variable password-buffer
variable password-length
variable attempts

create stored-password
  char f c, char o c, char r c, char t c, char h c,
5 constant stored-password-length

: check-password ( -- flag )
  password-length @ stored-password-length = if
    true
    password-length @ 0 ?do
      password-buffer i + c@
      stored-password i + c@
      <> if drop false leave then
    loop
  else
    false
  then
;

\ Fast rectangle fill using framebuffer
: fast-fill ( color x y w h -- )
  fb-stride * + 4 * fb-base + >r  \ Calculate start address
  
  0 ?do
    r@ j fb-stride * +            \ Row address
    over 4 * +                    \ Column offset
    2 pick                        \ Color
    3 pick 0 ?do                  \ Width loop
      2dup !
      4 +
    loop
    2drop
  loop
  r> drop 2drop
;

\ Show password prompt
: show-prompt-fast ( -- )
  \ Clear prompt area
  00000000 300 400 680 60 fast-fill
  
  \ Draw box
  ff808080 300 400 680 60 fast-fill
  ffffff00 300 400 680 4 fast-fill
  ffffff00 300 456 680 4 fast-fill
  ffffff00 300 400 4 60 fast-fill
  ffffff00 976 400 4 60 fast-fill
  
  \ Show asterisks
  password-length @ 0 ?do
    ffffffff 320 i 20 * + 420 12 12 fast-fill
  loop
;

\ Main fast lock screen
: lock-screen-fast ( -- )
  init-fast-display
  
  \ Clear screen
  fb-base screen-width screen-height * 4 * 0 fill
  
  \ Load background
  load-bmp-fast
  
  0 attempts !
  here password-buffer !
  
  \ Input loop
  begin
    show-prompt-fast
    key
    
    dup 13 = if  \ Enter
      drop
      check-password if
        " boot" evaluate
        exit
      else
        0 password-length !
        attempts @ 1+ attempts !
        attempts @ 3 >= if
          " shut-down" evaluate
          exit
        then
      then
    else
      dup 8 = if  \ Backspace
        drop
        password-length @ 0> if
          password-length @ 1- password-length !
        then
      else
        password-length @ 10 < if
          password-buffer password-length @ + c!
          password-length @ 1+ password-length !
        else
          drop
        then
      then
    then
  again
;

: test-fast ( -- )
  lock-screen-fast
;

." Crazytown Fast loaded. Use 'test-fast' to run." cr

decimal