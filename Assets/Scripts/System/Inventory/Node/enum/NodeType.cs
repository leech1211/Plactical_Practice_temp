﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[JsonConverter(typeof(StringEnumConverter))]
public enum NodeType
{
    NULL,
    RUN,
    INT_1,
    INT_2,
    INT_3,
    INT_4,
    INT_5,
    INT_6,
    INT_CUSTOM,
    INT_M1,
    INT_M2,
    INT_M3,
    FLOAT_1,
    FLOAT_2,
    FLOAT_3,
    FLOAT_CUSTOM,
    FLOAT_M1,
    FLOAT_M2,
    FLOAT_M3,
    BOOL_TRUE,
    BOOL_FALSE,
    CHAR_CUSTOM,
    OPERATOR_PLUS,
    OPERATOR_PLUS_INT,
    OPERATOR_MINUS,
    OPERATOR_MULTIPLY,
    OPERATOR_MULTIPLY_INT,
    OPERATOR_DIVISION,
    OPERATOR_MODULO,
    IF,
    IF_SKILL_HP,
    IF_SKILL_ENEMY,
    FOR,
    FOR_SKILL,
    WHILE_SKILL,
    SKILLSLOT1,
    SKILLSLOT2,
    SKILLSLOT3,
    OPERATOR,
    OPERATOR_INT,
    PLUS,
    MINUS,
    MULTIPLY,
    DIVISION,
    MODULO,
    GRATER,
    LESS,
    LESSEQUAL,
    GRATER_EQUAL,
    LESS_EQUAL,
    EQUAL,
    NOTEQUAL,
    AND,
    OR,
    NOT,
    IF_PROPERTY_RESULT,
    IF_PROPERTY_PLAYERHP,
    IF_PROPERTY_ALLYHP,
    IF_PROPERTY_ENEMYCOUNT,
    FOR_FLOAT,
    SET_POINTER,
    GET_POINTER,
    PLAYER_DEALER,
    PLAYER_MAGICIAN,
    PLAYER_TANKER,
    SKILL_POWERGUN,
    SKILL_POWERGUN_PROPERTY,
    SKILL_DASH,
    SKILL_DASH_PROPERTY,
    SKILL_LASER,
    SKILL_LASER_PROPERTY,
    SKILL_SHOOTINGSTAR,
    SKILL_SHOOTINGSTAR_PROPERTY,
    SKILL_HEALLIGHT,
    SKILL_HEALLIGHT_PROPERTY,
    SKILL_IMMORTALITYFIELD,
    SKILL_IMMORTALITYFIELD_PROPERTY,
    SKILL_POISONFIELD,
    SKILL_POISONFIELD_PROPERTY,
    SKILL_SWORDSLASH,
    SKILL_SWORDSLASH_PROPERTY,
    SKILL_CIRCLESHIELD,
    SKILL_CIRCLESHIELD_PROPERTY,
    SKILL_ROCKROAD,
    SKILL_ROCKROAD_PROPERTY,
    SHORT_GATLINGGUN_PROPERTY,
    WEEK1_0_DOOR,
    WEEK1_1_STONEPUSH_FORWARD,
    WEEK1_1_STONEPUSH_BACKWARD,
    WEEK1_2_SLOPEMOVE,
    WEEK1_3_BRIDGEMOVE,
    WEEK1_4_FANCE1,
    WEEK1_4_FANCE2,
    WEEK1_4_FANCE3,
    WEEK1_5_STONEPUSH_MOVEX,
    WEEK1_5_STONEPUSH_MOVEY,
    WEEK1_6_TOWERMOVE,
    WEEK1_7_SIGN1,
    WEEK1_7_SIGN2,
    WEEK1_7_SIGN3,
    WEEK1_7_EFFECT1,
    WEEK1_7_EFFECT2,
    WEEK1_7_EFFECT3,
    WEEK1_8_MAZERED,
    WEEK1_8_MAZEBLUE,
    WEEK1_8_MAZEYELLOW,
    WEEK1_9_ELEVATOR,
    WEEK1_10_SCALE_MULTIPLY,
    WEEK1_10_ARCH,
    WEEK1_11_STONE,
    WEEK1_12_GIANT,
    WEEK2_1_PROPBOMB_BOMB,
    WEEK2_1_PROPBOMB_MOVE,
    WEEK2_2_PROPBOMB_BOMB,
    WEEK2_2_PROPBOMB_MOVE,
    WEEK2_3_PROPBOMB_BOMB,
    WEEK2_3_PROPBOMB_FREEZE,
    WEEK2_4_IF_PUPLE,
    WEEK2_4_IF_STONE,
    WEEK2_4_IF_WHITE,
    WEEK2_4_PUPLE_TILE,
    WEEK2_4_SENSOR,
    WEEK2_4_STONE_TILE,
    WEEK2_4_WHITE_TILE,
    WEEK2_5_PROPBOMB_BOMB,
    WEEK2_5_PROPBOMB_BOMB2,
    WEEK2_5_PROPBOMB_BOMB3,
    WEEK2_6_PUPLE_TILE,
    WEEK2_6_TRAP_TILE,
    WEEK2_6_WHITE_TILE,
    WEEK2_7_FIRST_TILE,
    WEEK2_7_SECOND_TILE,
    WEEK2_7_THIRD_TILE,
    WEEK3_1_CURTIME,
    WEEK3_1_DRONE_FLOAT_1,
    WEEK3_1_DRONE_FLOAT_2,
    WEEK3_1_DRONE_FLOAT_3,
    WEEK3_1_DRONE_BOMB,
    WEEK3_1_DRONE_INT_1,
    WEEK3_1_DRONE_INT_2,
    WEEK3_1_DRONE_INT_3,
    WEEK3_1_DRONE_OPERATOR_PLUS,
    WEEK3_1_DRONE_OPERATOR_PLUS_INT,
    WEEK3_1_DRONE_OPERATOR_MULTIPLY,
    WEEK3_1_DRONE_OPERATOR_MULTIPLY_INT,
    WEEK3_1_DRONE_SHOT,
    WEEK3_1_FOR_DRONE_BOMB,
    WEEK3_1_LESSEQUAL,
    WEEK3_1_WHILE_DRONE_SHOT,
    WEEK3_2_DRONE_ATTACK,
    WEEK3_2_FOR_OBSTACLE,
    WEEK3_3_BRIDGE_LEFT_MOVE,
    WEEK3_3_BRIDGE_RIGHT_MOVE,
    WEEK3_3_WHILE_BRIDGE,
    WEEK3_4_FOR_RANGE,
    WEEK3_4_RANGEUP,
    WEEK3_4_BOMB,
    WEEK3_5_FOR_OBSTACLE,
    WEEK3_5_IF,
    WEEK3_5_REMAINDER,
    WEEK3_5_SHOT,
    WEEK3_5_CONTINUE,
    WEEK3_6_FOR_1,
    WEEK3_6_FOR_2,
    WEEK3_6_FOR_3,
    WEEK3_6_IF,
    WEEK3_6_BREAK,
    WEEK3_7_COLLECT_LAVA,
    WEEK3_7_CONTINUE,
    WEEK3_7_BREAK,
    WEEK3_7_SENSOR_LAVA,
    WEEK3_7_SENSOR_PRESSURE,
    WEEK4_1_SPAWN1,
    WEEK4_1_SPAWN2,
    WEEK4_2_RADAR1,
    WEEK4_2_RADAR2,
    WEEK4_3_EQUAL,
    WEEK4_4_IF,
    WEEK4_5_PROPERTY1,
    WEEK4_5_PROPERTY2,
    WEEK4_5_PROPERTY3,
    WEEK4_6_PLUS,
    WEEK5_1_PRINTF,
    WEEK5_1_A,
    WEEK5_1_B,
    WEEK5_1_X,
    WEEK5_1_Y,
    WEEK5_1_DECODING1,
    WEEK5_1_DECODING2,
    WEEK5_1_FUNCTION,
    WEEK5_1_STRCAT,
    WEEK5_2_MOVEUP,
    WEEK5_2_MOVEDOWN,
    WEEK5_2_MOVELEFT,
    WEEK5_2_MOVERIGHT,
    WEEK5_2_ROTATIONLEFT,
    WEEK5_2_ROTATIONRIGHT,
    WEEK5_2_FUNCTION,
}