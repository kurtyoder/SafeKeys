using SafeKeys.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace SafeKeys.Library
{
    public class GenerateStrings : IGenerateStrings
    {
        #region sample data

        private readonly string[] words = { "company", "likeable", "hurried", "unwieldy", "nimble", "illegal", "ski", "plan", "icy", "disagreeable", "tie", "natural", "perpetual", "sponge", "perform", "elderly", "knowledgeable", "illustrious", "range", "violent", "wonderful", "best", "stroke", "obsequious", "stitch", "committee", "periodic", "friendly", "barbarous", "third", "rejoice", "rob", "possible", "dare", "close", "luxuriant", "sleep", "amazing", "idea", "chickens", "title", "thirsty", "current", "prick", "flood", "magnificent", "steady", "scratch", "soda", "slippery", "fold", "bottle", "afternoon", "reproduce", "bomb", "puzzled", "remove", "massive", "lick", "moor", "spiders", "demonic", "nine", "board", "loutish", "learned", "laughable", "pass", "beginner", "grease", "zoo", "waste", "polish", "lamentable", "mate", "rate", "compete", "shelter", "fertile", "probable", "calm", "toad", "recognise", "well-groomed", "consist", "knit", "equal", "moon", "stir", "dad", "repulsive", "innocent", "maddening", "expand", "disappear", "legs", "light", "memorise", "develop", "muddled", "elfin", "amusement", "division", "voyage", "bloody", "elite", "flaky", "realise", "frantic", "neck", "picture", "unnatural", "fact", "wire", "hands", "living", "divergent", "equable", "rainy", "empty", "visitor", "wait", "smart", "statuesque", "size", "smoggy", "night", "colour", "secretive", "tense", "rinse", "haircut", "cobweb", "agreement", "fork", "squealing", "kneel", "functional", "sheep", "smell", "necessary", "cattle", "trip", "finicky", "license", "juice", "thundering", "scissors", "earthy", "alcoholic", "jaded", "secretary", "mute", "tub", "lettuce", "dashing", "snakes", "symptomatic", "ethereal", "crooked", "hellish", "apparel", "unkempt", "friend", "religion", "literate", "clumsy", "tent", "potato", "descriptive", "channel", "cumbersome", "books", "excuse", "floor", "ossified", "grate", "reading", "scatter", "defective", "bat", "marble", "ancient", "overt", "remarkable", "stretch", "crazy", "things", "rough", "acid", "mitten", "murder", "breath", "bashful", "driving", "aspiring", "box", "skinny", "spy", "ray", "dizzy", "hilarious", "silent", "copy", "fear", "pie", "boy", "subtract", "cap", "blue", "reign", "trousers", "ants", "brash", "chief", "vacuous", "regular", "normal", "admire", "fang", "new", "aquatic", "spring", "unruly", "sin", "cover", "pickle", "scarce", "real", "dolls", "ban", "own", "roomy", "injure", "sofa", "move", "peace", "salty", "bathe", "death", "delirious", "macho", "stupendous", "volleyball", "courageous", "skin", "unused", "glass", "therapeutic", "metal", "inexpensive", "experience", "rampant", "vast", "quicksand", "cook", "abandoned", "ruddy", "wood", "stereotyped", "nerve", "quarrelsome", "industry", "ten", "frail", "deceive", "curl", "disillusioned", "steep", "thick", "earsplitting", "free", "onerous", "ticket", "loaf", "sisters", "weak", "basket", "terrible", "card", "healthy", "save", "grape", "thoughtless", "summer", "threatening", "wall", "support", "recess", "hospitable", "gigantic", "art", "chop", "tug", "volatile", "coil", "wren", "penitent", "slope", "dream", "rural", "lovely", "wipe", "cup", "dogs", "plausible", "seal", "gleaming", "peck", "umbrella", "chicken", "walk", "gaping", "shelf", "ring", "fence", "longing", "hypnotic", "doubtful", "inconclusive", "zinc", "teeny", "sulky", "pick", "parcel", "possess", "heap", "pour", "paint", "wave", "warlike", "collar", "wealth", "whisper", "stone", "vein", "allow", "weigh", "cows", "fill", "rabbit", "thing", "labored", "buzz", "ritzy", "comfortable", "acceptable", "creepy", "truthful", "jolly", "sparkling", "kitty", "striped", "base", "request", "handsomely", "jumpy", "correct", "hover", "building", "lock", "hapless", "kick", "earn", "girl", "spill", "notice", "road", "unlock", "incompetent", "act", "sordid", "crib", "alert", "government", "bleach", "smelly", "caption", "meddle", "rail", "unwritten", "wild", "bored", "quack", "kettle", "short", "friends", "man", "organic", "cute", "pretend", "horrible", "mere", "old", "yoke", "ajar", "accidental", "improve", "parched", "annoyed", "whirl", "shade", "pin", "carpenter", "jazzy", "ignore", "list", "frightening", "coherent", "suspend", "condition", "enjoy", "exuberant", "pat", "selective", "insidious", "achiever", "aboriginal", "enter", "book", "railway", "abundant", "untidy", "silky", "wink", "female", "needle", "bite", "godly", "oatmeal", "medical", "obtainable", "chase", "bushes", "cheerful", "swift", "numberless", "car", "chubby", "slow", "contain", "permit", "charge", "amusing", "burly", "scent", "soggy", "possessive", "slim", "star", "store", "exotic", "acoustics", "sassy", "succinct", "exciting", "torpid", "shop", "roasted", "name", "extra-large", "flap", "naughty", "obtain", "fearless", "lowly", "harsh", "fail", "saw", "teaching", "hysterical", "one", "meek", "mine", "private", "song", "earth", "war", "celery", "brief", "existence", "carry", "talented", "rustic", "wiggly", "gorgeous", "room", "quarter", "scale", "efficient", "cry", "inquisitive", "actor", "activity", "tasty", "place", "return", "poor", "difficult", "destroy", "sea", "witty", "measure", "acrid", "fearful", "large", "welcome", "impartial", "shape", "glossy", "release", "like", "twig", "zesty", "huge", "imported", "hurry", "angle", "malicious", "next", "bike", "maid", "productive", "able", "hole", "skirt", "rice", "account", "bump", "quill", "water", "face", "rush", "amused", "puffy", "goofy", "town", "long-term", "venomous", "tawdry", "stupid", "ready", "overjoyed", "soak", "table", "clip", "overconfident", "poison", "soft", "nut", "paper", "rifle", "dependent", "science", "abrupt", "four", "toe", "grey", "giants", "bare", "lamp", "needless", "tomatoes", "prepare", "control", "truck", "needy", "start", "multiply", "team", "blushing", "pathetic", "print", "knot", "raspy", "vest", "maniacal", "cat", "good", "suspect", "direful", "creator", "confuse", "endurable", "poke", "wanting", "march", "concerned", "army", "concentrate", "stain", "zany", "connect", "tranquil", "cultured", "ducks", "ambitious", "shy", "trap", "train", "wax", "inform", "quilt", "insurance", "doubt", "upset", "rambunctious", "spare", "unfasten", "expect", "minute", "satisfy", "squirrel", "squeak", "steel", "collect", "boundless", "songs", "passenger", "gold", "number", "snatch", "profuse", "disastrous", "tooth", "part", "versed", "brass", "tight", "fade", "dislike", "deeply", "fast", "drown", "bounce", "compare", "examine", "north", "lie", "encourage", "joyous", "rare", "teeth", "cowardly", "hallowed", "erect", "soup", "school", "comparison", "identify", "wiry", "married", "ask", "extend", "cherries", "fasten", "efficacious", "glamorous", "quizzical", "nifty", "temper", "dirt", "vanish", "plough", "true", "tough", "tendency", "hospital", "string", "detect", "hour", "nondescript", "brown", "eager", "story", "gray", "judicious", "muscle", "approval", "broken", "toes", "rhetorical", "evasive", "far", "liquid", "interrupt", "force", "hill", "learn", "cloth", "worthless", "reduce", "abortive", "minor", "neat", "hope", "uttermost", "grade", "report", "delicate", "condemned", "eatable", "bite-sized", "snotty", "murky", "oil", "stocking", "supply", "help", "intelligent", "drip", "radiate", "whispering", "quick", "sip", "flash", "steer", "half", "tacky", "teeny-tiny", "well-to-do", "aback", "fry", "bone", "delicious", "disgusted", "tease", "airplane", "cool", "utter", "lying", "little", "offend", "arrive", "squeeze", "park", "watch", "mindless", "lonely", "handy", "pipe", "aggressive", "rabid", "rat", "material", "flawless", "average", "auspicious", "wash", "limping", "plant", "unique", "surprise", "obedient", "children", "decorous", "search", "clean", "romantic", "breezy", "addition", "robust", "nappy", "political", "domineering", "breathe", "chalk", "temporary", "visit", "invent", "idiotic", "different", "fluffy", "thoughtful", "mountain", "knowledge", "pray", "enchanted", "fortunate", "filthy", "itch", "increase", "spotty", "squeamish", "black-and-white", "adorable", "stew", "skillful", "step", "two", "abashed", "replace", "billowy", "jail", "paddle", "run", "successful", "grin", "interest", "bikes", "motionless", "offer", "pail", "clammy", "bore", "oval", "fresh", "scintillating", "aftermath", "adjoining", "inject", "complain", "humorous", "tangible", "hose", "abusive", "clam", "line", "strange", "flat", "left", "obscene", "color", "supreme", "curvy", "educated", "ragged", "stiff", "dynamic", "dance", "flower", "deranged", "desert", "male", "hug", "substance", "wandering", "quixotic", "racial", "duck", "somber", "turkey", "cactus", "unknown", "program", "charming", "boring", "tightfisted", "small", "spotless", "lazy", "weary", "extra-small", "cherry", "tender", "relax", "brainy", "unusual", "astonishing", "thought", "abnormal", "clap", "puny", "cub", "weather", "homeless", "bow", "jam", "noiseless", "sun", "nasty", "dinosaurs", "impress", "machine", "innate", "numerous", "back", "accept", "belong", "hard", "guttural", "gun", "spiteful", "bless", "splendid", "hammer", "feeble", "ink", "distribution", "unhealthy", "ambiguous", "mix", "tenuous", "tour", "pan", "guarded", "freezing", "psychotic", "better", "cluttered", "punish", "deserve", "spiffy", "flimsy", "drawer", "spoon", "week", "pull", "object", "time", "previous", "pump", "queue", "ill-informed", "scandalous", "hop", "flashy", "relation", "calendar", "point", "electric", "caring", "common", "big", "destruction", "mourn", "automatic", "reply", "sweltering", "ladybug", "transport", "camp", "month", "alarm", "coat", "jumbled", "purpose", "itchy", "aromatic", "borrow", "expansion", "fax", "mighty", "rotten", "appliance", "disapprove", "useful", "cheese", "frogs", "hang", "tart", "tumble", "baseball", "bright", "please", "tail", "scarecrow", "locket", "frighten", "green", "fretful", "drop", "file", "whip", "order", "abiding", "mom", "misty", "stage", "bake", "resolute", "repair", "ad hoc", "fine", "debonair", "crack", "slimy", "popcorn", "well-off", "grumpy", "appear", "incandescent", "wool", "open", "imaginary", "second-hand", "wind", "gabby", "jobless", "wheel", "sound", "dark", "telephone", "irritate", "ghost", "agree", "tap", "delightful", "calculator", "prefer", "chunky", "analyse", "glib", "level", "route", "bed", "sticky", "pet", "ice", "uptight", "yellow", "voracious", "high-pitched", "dress", "country", "treatment", "purring", "reject", "expensive", "animal", "hulking", "confused", "chew", "nauseating", "furtive", "education", "treat", "branch", "windy", "mass", "listen", "delight", "farm", "kindly", "glistening", "territory", "wander", "belief", "yarn", "axiomatic", "concern", "beef", "nail", "rings", "quaint", "draconian", "opposite", "alluring", "defiant", "melodic", "juvenile", "loving", "disarm", "fumbling", "delay", "frog", "chemical", "corn", "savory", "disturbed", "selfish", "melt", "continue", "terrify", "woozy", "harmonious", "placid", "simplistic", "competition", "absent", "jealous", "blue-eyed", "impossible", "shoe", "dock", "materialistic", "pocket", "yell", "even", "instrument", "obeisant", "bad", "jewel", "bustling", "curve", "uppity", "protective", "wide-eyed", "unadvised", "suggestion", "scrape", "class", "flight", "bait", "eggnog", "wide", "system", "lewd", "beautiful", "mammoth", "bolt", "faint", "claim", "fireman", "gather", "leather", "flame", "cooperative", "lighten", "remain", "ablaze", "dust", "many", "victorious", "remember", "swanky", "veil", "shaggy", "pest", "rot", "awful", "regret", "library", "unbiased", "enchanting", "argue", "elegant", "receipt", "tremendous", "fancy", "square", "scientific", "rhyme", "sudden", "hate", "middle", "connection", "tiresome", "lucky", "laborer", "market", "knee", "letter", "burst", "defeated", "ahead", "discussion", "smooth", "hungry", "rebel", "workable", "testy", "soap", "deadpan", "plantation", "cave", "ubiquitous", "produce", "error", "sky", "curved", "devilish", "trees", "agonizing", "development", "responsible", "spray", "button", "loud", "scold", "lace", "stem", "bath", "serious", "rich", "rose", "obese", "classy", "melted", "butter", "plucky", "fanatical", "tired", "tedious", "limit", "hobbies", "tiny", "tin", "reaction", "obey", "mess up", "honey", "early", "belligerent", "squash", "guiltless", "panicky", "mixed", "pen", "false", "pear", "miniature", "fair", "power", "scare", "smoke", "credit", "reach", "attraction", "great", "cycle", "loose", "dapper", "seemly", "describe", "bruise", "discreet", "glue", "toy", "zealous", "quickest", "overwrought", "high", "cheer", "plug", "window", "cars", "attack", "glow", "taste", "rock", "wealthy", "harass", "noxious", "divide", "homely", "careless", "physical", "hideous", "hollow", "wary", "cut", "foamy", "deliver", "absorbed", "alike", "trade", "pig", "eggs", "three", "edge", "overflow", "momentous", "vegetable", "bear", "sniff", "ashamed", "quartz", "event", "handle", "bizarre", "oven", "drum", "punch", "plain", "marked", "tremble", "admit", "bawdy", "example", "glorious", "parsimonious", "precious", "tree", "guarantee", "fuzzy", "daffy", "harbor", "believe", "adaptable", "cart", "hanging", "tax", "foregoing", "safe", "distinct", "unarmed", "noisy", "son", "moaning", "gifted", "stomach", "surround", "manage", "exultant", "long", "health", "imperfect", "permissible", "rightful", "verdant", "head", "pretty", "servant", "puncture", "straight", "bouncy", "faithful", "grain", "cream", "various", "clever", "toothpaste", "enormous", "thumb", "join", "trace", "pleasant", "need", "superb", "payment", "jagged", "marry", "doctor", "used", "partner", "future", "promise", "argument", "white", "blade", "thaw", "receptive", "pumped", "bitter", "noise", "ocean", "juggle", "confess", "toothsome", "taboo", "strong", "invite", "respect", "icky", "donkey", "prickly", "trucks", "mushy", "late", "holiday", "perfect", "knowing", "knock", "ugly", "office", "burn", "stop", "undress", "nostalgic", "ruin", "wrench", "plot", "press", "dime", "rule", "capable", "playground", "fly", "daughter", "useless", "juicy", "direction", "dazzling", "pleasure", "push", "decide", "sock", "oranges", "form", "stuff", "dusty", "bucket", "strip", "solid", "sophisticated", "economic", "typical", "tested", "spooky", "lopsided", "happy", "end", "closed", "fix", "property", "cough", "cemetery", "nervous", "cagey", "absurd", "brick", "cannon", "proud", "peel", "sink", "brave", "zipper", "thread", "protest", "crowd", "panoramic", "zippy", "look", "absorbing", "envious", "greedy", "cloudy", "meaty", "near", "debt", "expert", "bang", "futuristic", "hushed", "root", "fowl", "coast", "hum", "helpful", "lyrical", "desk", "depressed", "eye", "shirt", "paste", "mint", "excellent", "riddle", "tire", "linen", "call", "moan", "river", "separate", "wistful", "cynical", "boat", "copper", "sand", "odd", "trashy", "border", "wilderness", "boot", "pedal", "suggest", "abject", "yam", "accurate", "historical", "known", "deer", "land", "blow", "flowers", "ceaseless", "mist", "spiritual", "front", "fool", "messy", "hook", "avoid", "heavy", "outgoing", "mark", "narrow", "pigs", "sugar", "cellar", "waiting", "ruthless", "lumber", "curly", "cheat", "trust", "grandiose", "shut", "curtain", "include", "vase", "outstanding", "skip", "aware", "shiny", "nonchalant", "greet", "bent", "screeching", "flowery", "agreeable", "dog", "raise", "advice", "subsequent", "damaging", "berry", "film", "hateful", "arch", "island", "mundane", "famous", "advise", "sheet", "heat", "men", "gaze", "grateful", "youthful", "abstracted", "lake", "shake", "work", "yard", "determined", "representative", "quirky", "grass", "capricious", "brother", "girls", "flesh", "instinctive", "impolite", "planes", "aboard", "spotted", "jar", "pale", "encouraging", "beam", "selection", "ball", "kittens", "balance", "fantastic", "brawny", "moldy", "sturdy", "language", "flagrant", "lunchroom", "languid", "macabre", "familiar", "undesirable", "church", "jittery", "test", "sense", "royal", "can", "pushy", "helpless", "abounding", "faulty", "tickle", "post", "suit", "measly", "attractive", "recondite", "punishment", "deafening", "relieved", "coach", "standing", "lame", "verse", "interfere", "uncle", "excited", "distance", "sprout", "type", "sedate", "wise", "fuel", "spoil", "obsolete", "skate", "modern", "travel", "scene", "wonder", "drain", "geese", "sail", "rapid", "play", "creature", "escape", "hat", "spiky", "salt", "soothe", "cent", "value", "abrasive", "tacit", "cake", "suck", "five", "fire", "mug", "adventurous", "top", "succeed", "aberrant", "preserve", "rain", "tearful", "blind", "behavior", "poised", "switch", "deep", "null", "pets", "graceful", "reminiscent", "sloppy", "discover", "understood", "employ", "hard-to-find", "hot", "wreck", "certain", "knotty", "alleged", "yummy", "house", "painstaking", "present", "texture", "tangy", "snake", "aunt", "zephyr", "polite", "obnoxious", "rabbits", "record", "whole", "instruct", "cabbage", "offbeat", "bird", "unequal", "detail", "sweet", "icicle", "birthday", "miscreant", "roof", "match", "prose", "utopian", "towering", "groovy", "authority", "sincere", "chin", "quiet", "nose", "remind", "animated", "drab", "gentle", "friction", "eyes", "yak", "basin", "madly", "reflect", "valuable", "thankful", "circle", "abaft", "magic", "erratic", "trick", "tasteful", "cautious", "fluttering", "plate", "bury", "spurious", "shoes", "arm", "coal", "fallacious", "pencil", "growth", "consider", "influence", "slap", "merciful", "tray", "doll", "magenta", "winter", "rude", "whine", "hydrant", "zip", "embarrassed", "add", "gamy", "design", "memory", "eight", "notebook", "wholesale", "wing", "questionable", "crayon", "rely", "pies", "educate", "silly", "wine", "theory", "round", "plastic", "excite", "rod", "womanly", "sleet", "action", "basketball", "sister", "befitting", "fabulous", "boil", "psychedelic", "kiss", "pink", "shrug", "marvelous", "shiver", "queen", "scattered", "immense", "ratty", "appreciate", "launch", "fixed", "flow", "milk", "few", "milky", "happen", "hair", "minister", "vigorous", "stick", "lip", "trains", "stove", "choke", "decisive", "shocking", "airport", "picayune", "bead", "gruesome", "discovery", "yielding", "terrific", "waggish", "afraid", "door", "far-flung", "use", "lunch", "introduce", "dysfunctional", "pop", "gate", "horn", "zonked", "phobic", "telling", "judge", "parallel", "seat", "swim", "pack", "depend", "low", "unsightly", "hand", "mouth", "statement", "pot", "addicted", "ordinary", "guide", "wish", "care", "grieving", "giddy", "powerful", "cast", "grandmother", "scribble", "heal", "crook", "voiceless", "magical", "beg", "outrageous", "mysterious", "grandfather", "answer", "flag", "field", "heavenly", "unaccountable", "honorable", "horses", "sleepy", "evanescent", "zoom", "price", "purple", "vacation", "ground", "heartbreaking", "chivalrous", "vessel", "afterthought", "cow", "observation", "trite", "stream", "same", "plane", "husky", "rhythm", "trail", "reward", "cakes", "steam", "wriggle", "cushion", "ignorant", "nest", "guitar", "shallow", "sore", "breakable", "exercise", "throne", "jog", "label", "substantial", "bag", "straw", "blink", "exclusive", "mind", "want", "super", "wobble", "key", "scared", "scream", "sick", "childlike", "feigned", "repeat", "chance", "silver", "foot", "snobbish", "fish", "sable", "serve", "spade", "scarf", "energetic", "nippy", "sparkle", "accessible", "crow", "invincible", "rescue", "whimsical", "rainstorm", "preach", "tip", "wound", "cheap", "worry", "faded", "tow", "special", "crowded", "boiling", "aloof", "weight", "tan", "unit", "kaput", "staking", "change", "governor", "dirty", "twist", "sneaky", "meat", "didactic", "wakeful", "changeable", "women", "cause", "incredible", "boorish", "page", "load", "frame", "mature", "live", "warm", "foolish", "guard", "sigh", "houses", "dead", "badge", "beneficial", "grubby", "premium", "bit", "person", "stay", "blush", "intend", "camera", "cooing", "paltry", "robin", "babies", "oafish", "industrious", "dreary", "adamant", "heady", "insect", "hissing", "egg", "pizzas", "lean", "glove", "elbow", "violet", "harmony", "applaud", "legal", "kind", "side", "unable", "knife", "scary", "plants", "chess", "joke", "upbeat", "iron", "rake", "calculating", "sneeze", "adhesive", "fruit", "fall", "apparatus", "carve", "curious", "blood", "precede", "x-ray", "jeans", "profit", "strap", "tick", "seashore", "old-fashioned", "amuse", "broad", "fetch", "damp", "redundant", "fierce", "birds", "practise", "muddle", "troubled", "behave", "squeal", "right", "resonant", "shaky", "downtown", "crate", "amount", "dispensable", "slave", "fascinated", "word", "cable", "tall", "naive", "festive", "retire", "forgetful", "earthquake", "shame", "shave", "gusty", "nonstop", "general", "sad", "irritating", "rigid", "chilly", "reason", "cloistered", "occur", "snail", "air", "mask", "entertaining", "important", "detailed", "imagine", "disgusting", "satisfying", "embarrass", "giraffe", "awesome", "follow", "quince", "optimal", "guess", "vengeful", "station", "scrawny", "sack", "shrill", "subdued", "arithmetic", "peaceful", "fairies", "canvas", "conscious", "sharp", "tricky", "pricey", "ludicrous", "count", "grotesque", "arrogant", "crime", "day", "underwear", "woebegone", "pause", "giant", "petite", "red", "spot", "warn", "humdrum", "pine", "apathetic", "suppose", "nice", "thunder", "communicate", "spicy", "unsuitable", "history", "receive", "year", "spark", "amuck", "dramatic", "finger", "brake", "acoustic", "fat", "trouble", "six", "gaudy", "structure", "craven", "group", "sour", "mellow", "home", "grip", "irate", "thinkable", "lively", "feeling", "voice", "painful", "uninterested", "brush", "digestion", "stare", "suffer", "snore", "handsome", "fragile", "provide", "carriage", "humor", "beds", "dam", "neighborly", "apologise", "space", "bells", "kill", "bee", "cross", "wrap", "meal", "infamous", "dull", "touch", "busy", "spectacular", "ugliest", "worried", "tank", "complete", "mailbox", "loss", "greasy", "mend", "invention", "ship", "wacky", "squalid", "colossal", "alive", "jellyfish", "overrated", "unite", "puzzling", "wretched", "cracker", "simple", "garrulous", "pancake", "blot", "arrest", "majestic", "wet", "view", "degree", "mother", "mountainous", "stingy", "uncovered", "dry", "volcano", "lethal", "money", "matter", "business", "effect", "damage", "command", "rub", "wooden", "signal", "makeshift", "fog", "first", "note", "crabby", "explain", "seed", "annoy", "keen", "porter", "flock", "bell", "anxious", "leg", "wrist", "income", "elastic", "thank", "adjustment", "lavish", "spell", "pollution", "self", "pinch", "tiger", "tramp", "cats", "exchange", "announce", "show", "clover", "bewildered", "attempt", "damaged", "strengthen", "groan", "abhorrent", "letters", "lackadaisical", "diligent", "afford", "omniscient", "imminent", "enthusiastic", "drag", "clear", "wail", "motion", "horse", "decorate", "scorch", "crawl", "vulgar", "zebra", "mean", "grouchy", "try", "available", "miss", "bubble", "observe", "crown", "boundary", "toothbrush", "sticks", "young", "bumpy", "gullible", "boast", "holistic", "ultra", "jelly", "scrub", "phone", "drunk", "rest", "tidy", "explode", "orange", "smash", "hall", "funny", "hesitant", "club", "daily", "observant", "righteous", "sort", "comb", "vague", "thrill", "slip", "secret", "tasteless", "peep", "owe", "wry", "thin", "yawn", "pointless", "morning", "exist", "challenge", "coordinated", "attend", "stamp", "sidewalk", "furniture", "wrathful", "bedroom", "protect", "way", "laugh", "birth", "kindhearted", "dear", "powder", "snow", "hurt", "berserk", "colorful", "writing", "hunt", "prevent", "highfalutin", "love", "gratis", "internal", "ear", "smile", "careful", "settle", "habitual", "shivering", "truculent", "flippant", "eminent", "pastoral", "society", "harm", "advertisement", "woman", "awake", "snails", "approve", "nation", "haunt", "stranger", "writer", "mice", "nutty", "crash", "smiling", "talk", "position", "van", "engine", "wasteful", "frequent", "party", "easy", "whistle", "bulb", "annoying", "acidic", "decision", "share", "vagabond", "last", "cuddly", "disagree", "public", "watery", "sign", "question", "tongue", "crush", "stimulating", "unbecoming", "battle", "worm", "impulse", "desire", "angry", "complex", "unequaled", "well-made", "dinner", "arrange", "tempt", "waves", "vivacious", "ill-fated", "shock", "black", "ill", "street", "float", "combative", "full", "stormy", "steadfast", "reflective", "quiver", "deserted", "throat", "gainful", "double", "furry", "attach", "ripe", "found", "drink", "willing", "second", "lacking", "meeting", "bridge", "silk", "risk", "cruel", "frightened", "lumpy", "baby", "trot", "lush", "nutritious", "food", "decay", "turn", "unpack", "synonymous", "attract", "stale", "sweater", "piquant", "wrong", "grab", "wicked", "flavor", "oceanic", "interesting", "calculate", "wrestle", "entertain", "race", "anger", "nod", "swing", "cold", "monkey", "superficial", "jump", "dangerous", "fit", "uneven", "past", "cure", "roll", "refuse", "halting", "screw", "nebulous", "actually", "nosy", "military", "toys", "elated", "check", "callous", "assorted", "tame" };

        private readonly string[] bigWords = { "concinnating", "swaddle", "gawkier", "dolorimetry", "biparous", "jackyard", "borrowing", "kiddishness", "maternalized", "lederberg", "craniological", "superfarm", "celeste", "hardfistedness", "hebraist", "hypofunction", "consonantalizing", "outcompliment", "rudderpost", "predefault", "prospero", "okeechobee", "fittable", "undulled", "raspier", "counterpane", "ahuehuete", "carranza", "hermaphrodite", "frondless", "proximation", "nathaniel", "precorrespondent", "overtruly", "moonier", "germfree", "globular", "autotomize", "housekeep", "limitless", "unmobilised", "lighthead", "derleth", "interdome", "nonconstruable", "ungothic", "medicating", "intercombination", "hydrogenate", "macumba", "militate", "capture", "xanthine", "serologically", "photogram", "coeditor", "nonfaddist", "yodeler", "preguard", "warring", "inoffensive", "heliotyping", "typicalness", "thermoscope", "cakewalk", "imbower", "predeath", "preinclined", "odelsthing", "sensitize", "overcivil", "diazoalkane", "bretessï¿¥ï¾½", "multicentral", "arylating", "untantalized", "retract", "meliorate", "umpsteen", "medicament", "willowy", "superestablish", "pseudoapologetically", "unresuscitated", "satirized", "overhuman", "noninsistence", "diazole", "stereoisomeric", "mnesicles", "fisherman", "acinacifoliate", "analyzation", "behavioristic", "ptolemaist", "derogate", "cholerically", "annulus", "divagated", "currach", "undissoluble", "squaller", "amphicrania", "insalubrity", "terribleness", "tintometric", "unordinal", "superdebt", "woundless", "negatived", "valedictory", "enterozoon", "macroptic", "paraphrasis", "underverse", "speakably", "ironweed", "relocation", "truckler", "driftless", "hodometer", "postmark", "pneumococcus", "preobtruding", "legibleness", "antipathetic", "covenant", "nonsyntonical", "kwakiutl", "synaeresis", "unperpetuating", "immediateness", "caparison", "delectableness", "premarital", "parotitic", "inheritableness", "pickaxes", "neighborly", "sloganeer", "curarize", "cienfuegos", "unitariness", "pulpiter", "mandarin", "preexclude", "irksomeness", "analgesic", "unharangued", "hochheimer", "wakefulness", "vassalic", "dysesthetic", "impeachable", "unransacked", "convolute", "agglutinogenic", "acousmata", "balloonfishes", "hypogonadia", "subcutaneous", "diaphanousness", "fractable", "affinitive", "madrilne", "superacidity", "impetuously", "millwheel", "polycaste", "shenandoah", "boniness", "nongravitation", "defuzing", "parapsychological", "portraiture", "interrelated", "hecticly", "preoppose", "barnyard", "preeligible", "ambiguousness", "esmeraldas", "superqualified", "gasterocheires", "antherozoid", "fustigatory", "cordwood", "kisangani", "toadfishes", "numbness", "vigorous", "transistorizing", "exsiccative", "hypocrateriform", "unshrived", "momentum", "photobiotic", "rhodonite", "enzymolysis", "untolerated", "unstatable", "umbilical", "prerevenge", "centiares", "reincited", "overindustrialize", "albinistic", "becquerel", "obelizing", "quadrumane", "unidentifying", "huntingdonshire", "illtempered", "boliviano", "retestified", "sessional", "anthropomorphism", "nondisparity", "developement", "intercepter", "intermeet", "unexpeditable", "semiexclusive", "underchord", "dï¿¥ï¾¸sseldorf", "gomorrhean", "tartishly", "dischargeable", "saltpetre", "exiguously", "discriminatively", "decrepitating", "baboonish", "noncommerciality", "hatshepsut", "senarmontite", "kergulen", "musclebound", "unconsultable", "matholwych", "endoplasmic", "nonenvious", "ungloomily", "recapitalization", "subterfuge", "noncandescence", "peachlike", "suggestibleness", "pretelephone", "pontypool", "kremlinology", "animalism", "unchastity", "gadrooned", "underspin", "accessarily", "vasotonic", "bureaucracy", "sheiklike", "gladiolar", "corrosively", "succourer", "ungrowing", "nonbibulous", "washboard", "chairwoman", "toxicological", "footprint", "misinterpret", "showmanly", "misgivingly", "palynology", "cinematheque", "imputable", "uninviting", "monogamous", "gadolinium", "hastiness", "cornflour", "confutative", "glaciology", "multicorneal", "satanically", "paradisaic", "misidentify", "slinkingly", "irrationality", "unguttural", "stylobate", "overzealously", "unarbitrative", "encephalomyelitis", "astronomy", "overcaution", "unquavering", "dimensionality", "amendatory", "amphimarus", "stockwood", "objectivism", "oversupped", "tyrannizer", "distiller", "foreconscious", "overpromising", "proletarianise", "unrestored", "balloonfish", "prerighteous", "nonpurposive", "undersawyer", "bronislaw", "abhorrent", "antitaxation", "eradicable", "untastable", "scatteredness", "serologically", "predicating", "unbreathing", "demoiselle", "semirepublican", "excommunicative", "nationalist", "trophyless", "horologist", "revegetated", "nonspecific", "prussiate", "imparadising", "misplacing", "discreteness", "censurable", "preburlesque", "uncondensed", "agglutinogen", "hypnotisable", "phlogiston", "beriosova", "involutional", "anarchical", "supersatisfying", "unswayable", "enthusiast", "agromania", "dysthymia", "cyclotomic", "neopilina", "brownshirt", "hexastylar", "blockaded", "overprosperous", "overmilitaristically", "giganticness", "subinferred", "improvisatorial", "centrefold", "miserably", "slavophilism", "unsuppositional", "mutteringly", "proportional", "overimitated", "undescried", "anthologizing", "beresford", "unerringness", "homologizing", "corridored", "oberosterreich", "incomeless", "barnesville", "epiphanic", "overharden", "hackamore", "halmahera", "reutilization", "endometrium", "seditiousness", "overnormalize", "resolvableness", "discographically", "unreposing", "unexcluded", "presentation", "embranglement", "thermistor", "detoxicator", "collodion", "bisexualism", "transfusion", "joukahainen", "quadrilateral", "duodecimally", "disseizor", "zygophoric", "sagaciously", "bedeswoman", "playschool", "nonsubmergence", "conversable", "outsinging", "pinheaded", "colonised", "disentranced", "inescutcheon", "huffiness", "postnodular", "convincingness", "prestatistical", "overintense", "contraplete", "unsoporific", "underdown", "unpostponed", "plentiful", "unalerted", "polycrates", "prosthetist", "dyersville", "understay", "heartbreakingly", "nonignitible", "archetypically", "flageolet", "fortuitously", "preidentified", "antisymmetry", "recrimination", "unaccompanied", "disbursement", "incipient", "scottsbluff", "mckeesport", "nonparasitical", "yachtsmanship", "superliner", "capsulated", "trilateral", "helladian", "degenerative", "dispensability", "brigandine", "phonating", "nurseryman", "kingfisher", "cellulose", "undemolished", "lutherism", "antinomianism", "precombined", "incommutable", "burglarproof", "trisoctahedron", "nonchangeable", "unscolded", "roquelaure", "anthropography", "overcivilize", "steamship", "semicultivated", "assentingly", "nosepiece", "itineraries", "sigismund", "unpantheistic", "effeminized", "antifoaming", "dreariest", "pyridoxal", "tenderised", "unframeable", "sensibilia", "trichromat", "misproducing", "elemental", "advancingly", "undisqualified", "calibration", "cicatriser", "butterbur", "amatively", "unwatchable", "baroclinicity", "flannelboard", "liberalization", "matelass", "impromptu", "unnarrated", "discrepance", "superelastically", "monogamic", "subparietal", "pseudoviperous", "anchorite", "stockbroker", "digressiveness", "preidentify", "overdistention", "freezadrying" };

        private readonly string[] adjectives = { "creepy", "thoughtful", "abaft", "lacking", "unknown", "best", "overt", "childlike", "four", "alleged", "foregoing", "jealous", "brainy", "light", "callous", "fast", "macho", "erratic", "flashy", "marvelous", "purple", "like", "typical", "nostalgic", "hushed", "debonair", "rebel", "penitent", "upbeat", "unsuitable", "false", "noisy", "equable", "naughty", "melodic", "hurt", "hapless", "flaky", "amusing", "super", "short", "giant", "tacit", "mere", "terrific", "bored", "straight", "ablaze", "didactic", "big", "cute", "zonked", "stiff", "momentous", "rustic", "unwritten", "eager", "bouncy", "imaginary", "productive", "tense", "colorful", "sable", "resolute", "infamous", "damaging", "goofy", "sick", "squealing", "extra-small", "ancient", "splendid", "shy", "strange", "black", "plastic", "thirsty", "dazzling", "lean", "glossy", "sore", "intelligent", "deep", "excellent", "dangerous", "squeamish", "stupendous", "mute", "spotty", "secret", "warm", "gamy", "noxious", "cuddly", "previous", "famous", "ultra", "chilly", "doubtful", "knowing", "beautiful", "mighty", "bright", "funny", "vague", "temporary", "relieved", "cloistered", "wiggly", "obedient", "helpful", "jumbled", "incredible", "fretful", "thin", "educated", "miniature", "graceful", "kindly", "innate", "overjoyed", "amazing", "far", "fluttering", "windy", "oafish", "scattered", "dusty", "nonchalant", "heartbreaking", "crooked", "elegant", "standing", "fabulous", "vacuous", "flagrant", "cowardly", "jolly", "cheap", "chubby", "satisfying", "tranquil", "oceanic", "wonderful", "absent", "impolite", "soft", "testy", "equal", "necessary", "tightfisted", "steep", "lamentable", "large", "imminent", "slim", "brash", "whimsical", "next", "impartial", "repulsive", "dapper", "tested", "abrasive", "devilish", "smiling", "dirty", "far-flung", "demonic", "industrious", "wry", "verdant", "changeable", "helpless", "tricky", "first", "premium", "probable", "violet", "woozy", "small", "superb", "moldy", "fresh", "sassy", "pink", "youthful", "irritating", "scrawny", "plain", "aggressive", "lazy", "lying", "subdued", "itchy", "fluffy", "guiltless", "sneaky", "voiceless", "omniscient", "unsightly", "serious", "abnormal", "exclusive", "frightened", "obscene", "neighborly", "organic", "rotten", "whole", "telling", "trite", "scarce", "aback", "inexpensive", "tall", "outstanding", "jobless", "deafening", "boiling", "coordinated", "damaged", "dramatic", "innocent", "full", "deadpan", "aromatic", "good", "domineering", "cooing", "living", "wide-eyed", "nonstop", "aquatic", "dusty", "waiting", "likeable", "proud", "shaky", "hard-to-find", "talented", "impossible", "bite-sized", "skillful", "nippy", "known", "mushy", "utter", "friendly", "silky", "secretive", "gratis", "breakable", "evasive", "ossified", "faithful", "perpetual", "dashing", "yielding", "wrathful", "merciful", "disillusioned", "easy", "greasy", "pricey", "one", "eatable", "bad", "seemly", "spiteful", "abundant", "auspicious", "quaint", "unarmed", "near", "eight", "gainful", "mean", "bloody", "handy", "broken", "bawdy", "acidic", "elastic", "annoyed", "untidy", "chunky", "poised", "aware", "wiry", "expensive", "sloppy", "decorous", "staking", "low", "nimble", "spooky", "hanging", "dull", "truculent", "steady", "wet", "pointless", "subsequent", "cluttered", "royal", "loose", "nauseating", "pushy", "automatic", "ratty", "troubled", "two", "chief", "slow", "husky", "grumpy", "bent", "illustrious", "minor", "flowery", "distinct", "numberless", "early", "keen", "romantic", "wandering", "knotty", "swift", "tart", "sudden", "harmonious", "mysterious", "skinny", "white", "vulgar", "extra-large", "capable", "little", "scary", "material", "obsolete", "successful", "tense", "remarkable", "versed", "tired", "flawless", "tawdry", "thinkable", "great", "trashy", "orange", "amuck", "stormy", "heavy", "grandiose", "cruel", "psychedelic", "wistful", "awake", "military", "kind", "fair", "zany", "handsomely", "last", "abusive", "able", "well-groomed", "hot", "lonely", "unhealthy", "courageous", "futuristic", "ruddy", "mammoth", "tearful", "coherent", "jumpy", "modern", "scared", "tremendous", "tiny", "null", "silly", "roomy", "busy", "ragged", "draconian", "puzzled", "magnificent", "rough", "defective", "past", "cynical", "forgetful", "tasty", "broad", "panicky", "five", "miscreant", "wary", "panoramic", "mixed", "few", "macabre", "lovely", "spectacular", "ludicrous", "average", "terrible", "hysterical", "petite", "certain", "rigid", "unbecoming", "unruly", "colossal", "adhesive", "abstracted", "pleasant", "lowly", "adaptable", "abhorrent", "curly", "guarded", "sad", "periodic", "agonizing", "unnatural", "elite", "different", "obeisant", "giddy", "truthful", "ripe", "erect", "greedy", "left", "polite", "quickest", "second-hand", "grouchy", "finicky", "thoughtless", "needless", "slimy", "savory", "paltry", "accidental", "ruthless", "arrogant", "level", "entertaining", "knowledgeable", "materialistic", "groovy", "half", "angry", "tenuous", "aberrant", "therapeutic", "rare", "roasted", "precious", "lush", "divergent", "fixed", "messy", "tame", "nebulous", "elated", "redundant", "well-off", "quixotic", "fragile", "nondescript", "wicked", "violent", "curvy", "lewd", "hateful", "gleaming", "nutty", "strong", "rabid", "glib", "hissing", "evanescent", "brave", "habitual", "nice", "stale", "sweet", "misty", "homeless", "red", "private", "loutish" };

        private readonly string[] names = { "Alvin", "James", "Jim", "Colton", "Trey", "Deangelo", "Hai", "Benton", "Darrick", "Marlin", "Harlan", "Toney", "Adalberto", "Freddie", "Samual", "Neville", "Jeffery", "Bennett", "Ismael", "Leland", "Leon", "Lorenzo", "Tad", "Sidney", "Blair", "Dannie", "Royce", "Donny", "Theodore", "Guadalupe", "Linwood", "Graham", "Willian", "Marcos", "Billie", "Wade", "Vernon", "Branden", "Neil", "Mitchell", "Davis", "Jacinto", "Lyndon", "Reynaldo", "Javier", "Willy", "Ramon", "Ernesto", "Rosendo", "Jamel", "Domenica", "Penny", "Lynnette", "Hazel", "Rosalind", "Lissa", "Carline", "Maira", "Ruby", "Marlen", "Emmy", "Madaline", "Avis", "Paulita", "Vickie", "Lessie", "Georgeanna", "Elizebeth", "Audria", "Annalee", "Jaimie", "Margery", "Regena", "Miguelina", "Marhta", "Adelaide", "Mora", "Petronila", "Shemika", "Marvella", "Carlena", "Domitila", "Veda", "Nickie", "Ellie", "Alena", "Berta", "Evangelina", "Lilliam", "Patrica", "Darline", "Josefina", "Elizabeth", "Katelin", "Lakisha", "Kristan", "Bethanie", "Anamaria", "Glynda", "Daina", "Claudie", "Alethea", "Marybelle", "Vashti", "Peg", "Lawanna", "Rema", "Christene", "Katrina", "Leonia", "Beverley", "Pamela", "Caroline", "Tobie", "Mira", "Geneva", "Christia", "Alayna", "Janice", "Merrie", "Chana", "Lesa", "Katia", "Myriam", "Lura", "Nanci", "Clarice", "Dortha", "Heather", "Alvera", "Princess", "Leta", "Karlene", "Zenaida", "Deidre", "Latoria", "Else", "Starr", "Rosette", "Lauretta", "Tanesha", "Shonna", "Tennie", "Syreeta", "Evie", "Cristy", "Kristian", "Ellamae", "Faviola", "Iesha", "Burton", "Werner", "Reinaldo", "Vaughn", "Mathew", "Jermaine", "Raleigh", "Ezekiel", "Brett", "Shane", "Russell", "Miles", "Faustino", "Tony", "Marc", "Eduardo", "Julian", "Dominick", "Les", "Lawerence", "Donn", "Arron", "Eli", "Herb", "Kieth", "Murray", "Tyron", "Neville", "Patricia", "Elliot", "Harley", "Dong", "Boyd", "Teddy", "Cameron", "Cristobal", "Francisco", "Johnathan", "Hal", "Ed", "Buster", "Randall", "Weston", "Kurt", "Bart", "Douglass", "Gilbert", "Jimmy", "Clayton", "Wilfred" };

        #endregion sample data

        private readonly string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

        private readonly string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        private readonly string[] regularCharacters = { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "+", "=" };

        private readonly string[] irregularCharacters = { "~", "`", "{", "[", "}", "]", "|", "\"", ":", ";", "\"", "'", "<", ",", ">", ".", "?", "/" };

        public string Generate(StringGenerationModel g)
        {
            var gen = new StringGenerationModel(g.Adj, g.Name, g.Word, g.BigWord, g.RegChar, g.IrregChar, g.Letter, g.Number, g.RandomCaps, g.FakeWord);

            string result = string.Empty;

            var r = new Random();

            var available = new List<int>();

            if (gen.Adj > 0)
                available.Add(0);
            if (gen.Name > 0)
                available.Add(1);
            if (gen.Word > 0)
                available.Add(2);
            if (gen.BigWord > 0)
                available.Add(3);
            if (gen.RegChar > 0)
                available.Add(4);
            if (gen.IrregChar > 0)
                available.Add(5);
            if (gen.Letter > 0)
                available.Add(6);
            if (gen.Number > 0)
                available.Add(7);
            if (gen.FakeWord > 0)
                available.Add(8);

            while (gen.Adj > 0 || gen.Name > 0 || gen.Word > 0 || gen.BigWord > 0 || gen.RegChar > 0 || gen.IrregChar > 0 || gen.Letter > 0 || gen.Number > 0 || gen.FakeWord > 0)
            {
                string addText = string.Empty;

                switch (available[GenerateRandomNumber(0, available.Count)])
                {
                    case 0:
                        addText = adjectives[GenerateRandomNumber(0, adjectives.Length)];
                        gen.Adj -= 1;
                        if (gen.Adj < 1)
                            _ = available.Remove(0);
                        break;

                    case 1:
                        addText = names[GenerateRandomNumber(0, names.Length)];
                        gen.Name -= 1;
                        if (gen.Name < 1)
                            _ = available.Remove(1);
                        break;

                    case 2:
                        addText = words[GenerateRandomNumber(0, words.Length)];
                        gen.Word -= 1;
                        if (gen.Word < 1)
                            _ = available.Remove(2);
                        break;

                    case 3:
                        addText = bigWords[GenerateRandomNumber(0, bigWords.Length)];
                        gen.BigWord -= 1;
                        if (gen.BigWord < 1)
                            _ = available.Remove(3);
                        break;

                    case 4:
                        addText = regularCharacters[GenerateRandomNumber(0, regularCharacters.Length)];
                        gen.RegChar -= 1;
                        if (gen.RegChar < 1)
                            _ = available.Remove(4);
                        break;

                    case 5:
                        addText = irregularCharacters[GenerateRandomNumber(0, irregularCharacters.Length)];
                        gen.IrregChar -= 1;
                        if (gen.IrregChar < 1)
                            _ = available.Remove(5);
                        break;

                    case 6:
                        addText = letters[GenerateRandomNumber(0, letters.Length)];
                        gen.Letter -= 1;
                        if (gen.Letter < 1)
                            _ = available.Remove(6);
                        break;

                    case 7:
                        addText = numbers[GenerateRandomNumber(0, numbers.Length)];
                        gen.Number -= 1;
                        if (gen.Number < 1)
                            _ = available.Remove(7);
                        break;

                    case 8:
                        addText = GenerateFakeWord(6);
                        gen.FakeWord -= 1;
                        if (gen.Number < 1)
                            _ = available.Remove(8);
                        break;
                }

                if (gen.RandomCaps)
                {
                    string adjustedText = string.Empty;

                    for (int i = 0; i < addText.Length; i++)
                    {
                        adjustedText += GenerateRandomNumber(0, 3) == 1 ? addText[i].ToString().ToUpper() : addText[i].ToString();
                    }

                    addText = adjustedText;
                }


                result += addText;
            }

            return result;
        }

        private readonly string[] vol = { "a", "e", "i", "o", "u" };
        private readonly string[] comb = { "ch", "ck", "kn", "sh", "qu" };
        private readonly string[] start = { "in", "im", "multi", "de", "over", "uni", "post", "pre", "un", "non", "bi", "quad", "di", "ana", "mis", "anti", "pneu", "bi", "a" };
        private readonly string[] end = { "y", "es", "ly", "able", "est", "ce", "ing", "ous", "ed", "ology", "ion", "tic", "ism", "ize", "ied", "ify", "ic", "ouse", "er", "ess", "ness" };

        private readonly string[] bla = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };

        private string GenerateFakeWord(int charLengthMin)
        {
            string result = string.Empty;

            var r = new Random();

            //Start word

            if (GenerateRandomNumber(2) == 1)
            {
                //Use beginning
                result += start[GenerateRandomNumber(start.Length)];
            }

            //Body

            do
            {
                if (GenerateRandomNumber(4) == 3)
                {
                    string w = comb[GenerateRandomNumber(comb.Length)];

                    while (result.Contains(w))
                    {
                        w = comb[GenerateRandomNumber(comb.Length)];
                    }

                    result += w;
                }
                else
                {
                    result += bla[GenerateRandomNumber(bla.Length)];
                }

                if (GenerateRandomNumber(4) == 3)
                {
                    result += vol[GenerateRandomNumber(vol.Length)];
                    result += vol[GenerateRandomNumber(vol.Length)];
                }
                else
                {
                    result += vol[GenerateRandomNumber(vol.Length)];
                }
            } while (result.Length - 1 <= charLengthMin);

            if (GenerateRandomNumber(4) == 3)
            {
                string w = comb[GenerateRandomNumber(comb.Length)];

                while (result.Contains(w))
                {
                    w = comb[GenerateRandomNumber(comb.Length)];
                }

                result += w;
            }
            else
            {
                result += bla[GenerateRandomNumber(bla.Length)];
            }

            //End word
            if (GenerateRandomNumber(2) == 1)
            {
                //Use beginning
                result += end[GenerateRandomNumber(end.Length)];
            }

            return result;
        }

        private int GenerateRandomNumber(int maxValue)
        {
            return GenerateRandomNumber(0, maxValue);
        }

        private int GenerateRandomNumber(int minValue, int maxValue)
        {
            // We will make up an integer seed from 4 bytes of this array.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert four random bytes into a positive integer value.
            int seed = ((randomBytes[0] & 0x7f) << 24) |
                        (randomBytes[1] << 16) |
                        (randomBytes[2] << 8) |
                        (randomBytes[3]);

            // Now, this looks more like real randomization.
            var random = new Random(seed);

            // Calculate a random number.
            return random.Next(minValue, maxValue);
        }
    }

  
}